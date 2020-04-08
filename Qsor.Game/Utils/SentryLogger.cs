using System;
using osu.Framework.Development;
using osu.Framework.Logging;
using Sentry;

namespace Qsor.Game.Utils
{
    public class SentryLogger : IDisposable
    {
        private readonly SentryClient _sentry;

        public SentryLogger(QsorBaseGame game)
        {
            if (DebugUtils.IsDebugBuild)
                return;
            
            var sentryOptions = new SentryOptions
            {
                Dsn = new Dsn("https://ddefbeb05e074f9c993bf1a72eb2a602@o169266.ingest.sentry.io/5193034"),
                Release = game.Version
            };
            
            _sentry = new SentryClient(sentryOptions);
            var sentryScope = new Scope(sentryOptions);
            
            Exception lastException = null;
            Logger.NewEntry += async entry =>
            {
                if (entry.Level < LogLevel.Verbose)
                    return;

                var exception = entry.Exception;

                if (exception != null)
                {
                    if (lastException != null && // We shouldn't resubmit the same exception
                        lastException.Message == exception.Message &&
                        exception.StackTrace?.StartsWith(lastException.StackTrace ?? string.Empty) == true)
                        return;
                    
                    _sentry.CaptureEvent(new SentryEvent(exception) { Message = entry.Message }, sentryScope);
                    lastException = exception;
                }
                else
                {
                    sentryScope.AddBreadcrumb(DateTimeOffset.Now, entry.Message, entry.Target.ToString(), "qsor-logger");
                }
            };
        }
        
        public void Dispose()
        {
            _sentry?.Dispose();
        }
    }
}