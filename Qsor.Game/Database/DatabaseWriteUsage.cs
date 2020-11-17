using System;
using System.Collections.Generic;

namespace Qsor.Game.Database
{
    // Modified version of https://github.com/ppy/osu/blob/master/osu.Game/Database/DatabaseWriteUsage.cs under MIT License!
    public class DatabaseWriteUsage : IDisposable
    {
        public readonly QsorDbContext Context;
        private readonly Action<DatabaseWriteUsage> _usageCompleted;
        public readonly List<Exception> Errors = new();

        private bool _isDisposed;

        /// <summary>
        ///     Whether this write usage will commit a transaction on completion.
        ///     If false, there is a parent usage responsible for transaction commit.
        /// </summary>
        public bool IsTransactionLeader = false;

        public DatabaseWriteUsage(QsorDbContext context, Action<DatabaseWriteUsage> onCompleted)
        {
            Context = context;
            _usageCompleted = onCompleted;
        }

        public bool PerformedWrite { get; private set; }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected void Dispose(bool disposing)
        {
            if (_isDisposed)
                return;

            _isDisposed = true;

            try
            {
                PerformedWrite |= Context.SaveChanges() > 0;
            } catch (Exception e)
            {
                Errors.Add(e);
                throw;
            }
            finally
            {
                _usageCompleted?.Invoke(this);
            }
        }

        ~DatabaseWriteUsage()
        {
            Dispose(false);
        }
    }
}