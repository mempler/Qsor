using System.Threading;
using Microsoft.EntityFrameworkCore.Internal;
using Microsoft.EntityFrameworkCore.Storage;
using osu.Framework.Platform;

namespace Qsor.Database
{
    // Modified version of https://github.com/ppy/osu/blob/master/osu.Game/Database/DatabaseContextFactory.cs under MIT License!
    public class QsorDbContextFactory
    {
        private readonly Storage _storage;
        private readonly object _writeLock = new object();
        private bool _currentWriteDidError;

        private bool _currentWriteDidWrite;

        private IDbContextTransaction _currentWriteTransaction;

        private int _currentWriteUsages;
        private ThreadLocal<QsorDbContext> _threadContexts;

        public QsorDbContextFactory(Storage storage)
        {
            _storage = storage;
            RecycleThreadContexts();
        }

        /// <summary>
        ///     Get a context for the current thread for read-only usage.
        ///     If a <see cref="DatabaseWriteUsage" /> is in progress, the existing write-safe context will be returned.
        /// </summary>
        public QsorDbContext Get() => _threadContexts.Value;

        /// <summary>
        ///     Request a context for write usage. Can be consumed in a nested fashion (and will return the same underlying
        ///     context).
        ///     This method may block if a write is already active on a different thread.
        /// </summary>
        /// <param name="withTransaction">Whether to start a transaction for this write.</param>
        /// <returns>A usage containing a usable context.</returns>
        public DatabaseWriteUsage GetForWrite(bool withTransaction = true)
        {
            Monitor.Enter(_writeLock);
            QsorDbContext context;

            try
            {
                if (_currentWriteTransaction == null && withTransaction)
                {
                    // this mitigates the fact that changes on tracked entities will not be rolled back with the transaction by ensuring write operations are always executed in isolated contexts.
                    // if this results in sub-optimal efficiency, we may need to look into removing Database-level transactions in favour of running SaveChanges where we currently commit the transaction.
                    if (_threadContexts.IsValueCreated)
                        RecycleThreadContexts();

                    context = _threadContexts.Value;
                    _currentWriteTransaction = context.Database.BeginTransaction();
                }
                else
                {
                    // we want to try-catch the retrieval of the context because it could throw an error (in CreateContext).
                    context = _threadContexts.Value;
                }
            } catch
            {
                // retrieval of a context could trigger a fatal error.
                Monitor.Exit(_writeLock);
                throw;
            }

            Interlocked.Increment(ref _currentWriteUsages);

            return new DatabaseWriteUsage(context, UsageCompleted)
            {
                IsTransactionLeader = _currentWriteTransaction != null && _currentWriteUsages == 1
            };
        }

        private void UsageCompleted(DatabaseWriteUsage usage)
        {
            var usages = Interlocked.Decrement(ref _currentWriteUsages);

            try
            {
                _currentWriteDidWrite |= usage.PerformedWrite;
                _currentWriteDidError |= usage.Errors.Any();

                if (usages == 0)
                {
                    if (_currentWriteDidError)
                        _currentWriteTransaction?.Rollback();
                    else
                        _currentWriteTransaction?.Commit();

                    if (_currentWriteDidWrite || _currentWriteDidError)
                    {
                        // explicitly dispose to ensure any outstanding flushes happen as soon as possible (and underlying resources are purged).
                        usage.Context.Dispose();

                        // once all writes are complete, we want to refresh thread-specific contexts to make sure they don't have stale local caches.
                        RecycleThreadContexts();
                    }

                    _currentWriteTransaction = null;
                    _currentWriteDidWrite = false;
                    _currentWriteDidError = false;
                }
            }
            finally
            {
                Monitor.Exit(_writeLock);
            }
        }

        private void RecycleThreadContexts()
        {
            // Contexts for other threads are not disposed as they may be in use elsewhere. Instead, fresh contexts are exposed
            // for other threads to use, and we rely on the finalizer inside SoraDbContext to handle their previous contexts
            _threadContexts?.Value.Dispose();
            _threadContexts = new ThreadLocal<QsorDbContext>(CreateContext, true);
        }

        protected virtual QsorDbContext CreateContext()
            => new QsorDbContext(_storage.GetDatabaseConnectionString("qsor")) {Database = {AutoTransactionsEnabled = false}};

        public void ResetDatabase()
        {
            lock (_writeLock)
            {
                RecycleThreadContexts();
                try
                {
                    _storage.DeleteDatabase("qsor");
                }
                catch
                {
                    // for now we are not sure why file handles are kept open by EF, but this is generally only used in testing
                }
            }
        }
    }
}