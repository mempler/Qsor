using System;
using osu.Framework.Development;
using osu.Framework.Logging;
using Sentry;

namespace Qsor.Game.Utils
{
    public class SentryLogger : IDisposable
    {
        private IDisposable _sentry;

        public SentryLogger(QsorBaseGame game)
        {
            if (DebugUtils.IsDebugBuild)
                return;

            _sentry = SentrySdk.Init(o =>
            {
                o.Dsn = "https://ddefbeb05e074f9c993bf1a72eb2a602@o169266.ingest.sentry.io/5193034";
                o.Release = QsorBaseGame.Version;
            });
            
            Exception lastException = null;
            Logger.NewEntry += entry =>
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

                    SentrySdk.CaptureEvent(new SentryEvent(exception) { Message = entry.Message });
                    lastException = exception;
                }
                else
                {
                    SentrySdk.AddBreadcrumb(entry.Message, entry.Target.ToString(), "qsor-logger");
                }
            };
        }
        
        public void Dispose()
        {
            _sentry?.Dispose();
        }
    }
}