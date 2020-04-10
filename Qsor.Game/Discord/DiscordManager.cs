using System;
using osu.Framework.Allocation;
using osu.Framework.Bindables;
using osu.Framework.Graphics;
using osu.Framework.Logging;
using Qsor.Game.Discord.GameSDK;
using LogLevel = Qsor.Game.Discord.GameSDK.LogLevel;

namespace Qsor.Game.Discord
{
    public enum DiscordActivityStatus
    {
        Default,
        Idle
    }
    
    public class DiscordManager : Component
    {
        public readonly Bindable<DiscordActivityStatus> BindableDiscordActivityStatus = new Bindable<DiscordActivityStatus>();
        private GameSDK.Discord _gameSdk;

        [BackgroundDependencyLoader]
        private void Load()
        {
            BindableDiscordActivityStatus.ValueChanged += e => HandleChangeActivityStatus(e.NewValue);

            // Discord Game SDK is not thread safe, it must run on the Update Thread
            Schedule(InitializeDiscord);
        }

        private void HandleChangeActivityStatus(DiscordActivityStatus activityStatus)
        {
            if (_gameSdk == null)
                return;
            
            switch (activityStatus)
            {
                case DiscordActivityStatus.Default:
                    _gameSdk
                        .GetActivityManager()
                        .UpdateActivity(new Activity
                        {
                            Name = "Qsor",
                            Details = $"Running Qsor {QsorBaseGame.Version}",
                            Assets = new ActivityAssets
                            {
                                LargeImage = "logo",
                                LargeText = "Qsor"
                            },
                            State = "cup o’ cheater tears"
                        }, _ => { });
                    break;
                case DiscordActivityStatus.Idle:
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        private void InitializeDiscord()
        {
            Scheduler.CancelDelayedTasks();
            
            try // i don't like to use try catch but it needs to be done in this case.
            {
                _gameSdk = new GameSDK.Discord(694816216442863667, (int) CreateFlags.NoRequireDiscord);
                _gameSdk.SetLogHook(LogLevel.Debug,
                    (_, value) => Logger.LogPrint($"Discord: {value}", LoggingTarget.Network, osu.Framework.Logging.LogLevel.Debug));
                    
                Scheduler.AddOnce(_gameSdk.RunCallbacks);

                BindableDiscordActivityStatus.Value = DiscordActivityStatus.Idle;
                BindableDiscordActivityStatus.Value = DiscordActivityStatus.Default;
            }
            catch (ResultException ex)
            {
                Scheduler.AddDelayed(InitializeDiscord, 5000, true);
                
                if (ex.Result == Result.InternalError) // due to a bug in the GameSDK, it'll throw an internal error.
                    Logger.LogPrint("Discord is not Running!", LoggingTarget.Network);
                else
                    Logger.Error(ex, "Discord GameSDK threw an Error", LoggingTarget.Network);
            }
        }
    }
}