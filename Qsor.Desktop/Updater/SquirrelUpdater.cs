using System;
using System.Net;
using System.Threading;
using Newtonsoft.Json;
using osu.Framework.Allocation;
using osu.Framework.IO.Network;
using osu.Framework.Platform;
using Qsor.Game.Updater;
using Squirrel;

namespace Qsor.Desktop.Updater
{
    [Cached]
    public class SquirrelUpdater : Game.Updater.Updater
    {
        [Resolved]
        private Storage Storage { get; set; }

        private UpdateManager _updateManager;
        

        [BackgroundDependencyLoader]
        private async void Load()
        {
            var jwr = new JsonWebRequest<GitHubRelease>("https://api.github.com/repos/osuAkatsuki/Qsor/releases/latest");
            
            await jwr.PerformAsync();
            
            _updateManager = new UpdateManager($"https://github.com/osuAkatsuki/Qsor/releases/download/{jwr.ResponseObject.TagName}/");
        }
        
        public override async void CheckAvailable()
        {
            while (_updateManager == null)
                Thread.Sleep(1);

            var updateInfo = await _updateManager.CheckForUpdate();

            if (updateInfo.ReleasesToApply.Count > 0)
                BindableStatus.Value = UpdaterStatus.Pending;
        }

        private bool _hasStarted;
        public override async void UpdateGame()
        {
            if (_hasStarted)
                return;
            
            BindableStatus.Value = UpdaterStatus.Downloading;

            _hasStarted = true;
            
            await _updateManager.UpdateApp(progress => BindableProgress.Value = progress);
        }
        
        public class GitHubRelease
        {
            [JsonProperty("tag_name")]
            public string TagName { get; set; }
        }
    }
}