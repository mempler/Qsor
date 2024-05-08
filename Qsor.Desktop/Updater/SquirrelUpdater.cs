using System;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Runtime.Versioning;
using System.Threading;
using Newtonsoft.Json;
using osu.Framework.Allocation;
using osu.Framework.Platform;
using Qsor.Game.Updater;
using Squirrel;
using Squirrel.Sources;

namespace Qsor.Desktop.Updater
{
    [Cached]
    [SupportedOSPlatform("windows")]
    public partial class SquirrelUpdater : Game.Updater.Updater, IDisposable
    {
        [Resolved]
        private Storage Storage { get; set; }
        
        [Resolved]
        private GameHost Host { get; set; }

        private UpdateManager _updateManager;
        

        [BackgroundDependencyLoader]
        private void Load()
        {
            const string GITHUB_ACCESS_TOKEN = null; // TODO: fill in your GitHub access token here

            _updateManager = new UpdateManager(new GithubSource("https://github.com/mempler/Qsor/", GITHUB_ACCESS_TOKEN, true), "qsor");
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
            if (BindableStatus.Value == UpdaterStatus.Ready)
            {
                var entry = Path.GetDirectoryName(Assembly.GetEntryAssembly()?.Location);
                if (entry == null)
                    throw new NullReferenceException();
                
                var path = Path.Join(entry, "../");
                
                Console.WriteLine(path);

                Process.Start(Path.Join(path, "./Qsor.exe"), "--updated");

                Host.Exit();
            }
                        
            if (_hasStarted)
                return;
            
            BindableStatus.Value = UpdaterStatus.Downloading;

            _hasStarted = true;
            
            await _updateManager.UpdateApp(progress => BindableProgress.Value = progress);

            BindableStatus.Value = UpdaterStatus.Ready;
        }

        protected override void Dispose(bool isDisposing)
        {
            base.Dispose(isDisposing);
            _updateManager?.Dispose();
        }

        public class GitHubRelease
        {
            [JsonProperty("tag_name")]
            public string TagName { get; set; }
        }
    }
}
