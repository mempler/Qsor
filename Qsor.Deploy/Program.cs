using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net.Http;
using Newtonsoft.Json;
using osu.Framework.IO.Network;
using osu.Framework.Platform;

namespace Qsor.Deploy
{
    internal static class Program
    {
        private static string NugetPackages => Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile), @".nuget\packages");
        private static string NugetPath => Path.Combine(NugetPackages, @"nuget.commandline\5.6.0\tools\NuGet.exe");
        private static string SquirrelPath => Path.Combine(NugetPackages, @"ppy.squirrel.windows\1.9.0.4\tools\Squirrel.exe");

        private static string GithubAccessToken => Environment.GetEnvironmentVariable("GITHUB_ACCESS_TOKEN");

        private static void Main()
        {
            var currentDirectory = new NativeStorage(".");
            var solutionDirectory = new NativeStorage("..");
            
            currentDirectory.DeleteDirectory("./staging");

            if (!Directory.Exists("releases"))
                Directory.CreateDirectory("releases");

            var stagingDirectory = currentDirectory.GetStorageForDirectory("staging");
            var currentDate = DateTime.Now.ToString("yyyy.Mdd.");
            
            currentDate += currentDirectory.GetDirectories("releases").Count(s => s.Contains(currentDate));

            var releaseDirectory = currentDirectory.GetStorageForDirectory($"./releases/App-{currentDate}");

            Console.WriteLine($"Package: Qsor");
            Console.WriteLine($"Release Version: {currentDate}");
            Console.WriteLine($"Release Directory: {releaseDirectory.GetFullPath(".")}");
            Console.WriteLine($"Changelog: \n{ChangelogGenerator.GenerateChangelog()}");

            var logo = solutionDirectory.GetFullPath("Qsor.Game/Resources/Textures/Logo-256x256.png");
            var icon = solutionDirectory.GetFullPath("Qsor.Desktop/icon.ico");
            
            RunCommand("dotnet", $"publish -f net5.0 Qsor.Desktop --configuration Release --runtime win-x64 -p:Version={currentDate} -o {stagingDirectory.GetFullPath(".")}",
                solutionDirectory.GetFullPath("."));
            
            RunCommand("./tools/rcedit-x64.exe", $"\"{stagingDirectory.GetFullPath(".")}\\Qsor.exe\" --set-icon \"{icon}\"");

            RunCommand(NugetPath, $"pack Qsor.Desktop/qsor.nuspec -Version {currentDate} -Properties Configuration=Release -OutputDirectory {stagingDirectory.GetFullPath(".")} -BasePath {stagingDirectory.GetFullPath(".")}",
                solutionDirectory.GetFullPath("."));
            
            RunCommand(SquirrelPath, $"--releasify {stagingDirectory.GetFullPath($"./Qsor.{currentDate}.nupkg")} --releaseDir {releaseDirectory.GetFullPath(".")} --no-msi --icon {icon} --setupIcon {icon} --loadingGif {logo}",
                stagingDirectory.GetFullPath("."));

            RunCommand("git", $"tag {currentDate}");
            RunCommand("git", $"push origin {currentDate}");

            File.Move(releaseDirectory.GetFullPath("Setup.exe"), releaseDirectory.GetFullPath("install.exe"));
            
            stagingDirectory.DeleteDirectory(".");
            
            var req = new JsonWebRequest<GitHubRelease>($"https://api.github.com/repos/Chimu-moe/Qsor/releases")
            {
                Method = HttpMethod.Post,
            };
            
            Console.WriteLine($"Creating release {currentDate}...");

            req.AddRaw(JsonConvert.SerializeObject(new GitHubRelease
            {
                Name = currentDate,
                Draft = true,
                Body = ChangelogGenerator.GenerateChangelog()
            }));
            
            req.AddHeader("Authorization", $"token {GithubAccessToken}");
            req.Perform();

            var targetRelease = req.ResponseObject;
            
            var assetUploadUrl = targetRelease.UploadUrl.Replace("{?name,label}", "?name={0}");
            foreach (var a in Directory.GetFiles(releaseDirectory.GetFullPath(".")).Reverse())
            {
                if (Path.GetFileName(a).StartsWith('.'))
                    continue;

                Console.WriteLine($"- Pushing asset {a}...");
                var upload = new WebRequest(assetUploadUrl, Path.GetFileName(a))
                {
                    Method = HttpMethod.Post,
                    Timeout = 240000,
                    ContentType = "application/octet-stream",
                };

                upload.AddRaw(File.ReadAllBytes(a));
                upload.AddHeader("Authorization", $"token {GithubAccessToken}");
                upload.Perform();
            }
        }

        private static void RunCommand(string command, string args, string workingDirectory = null)
        {
            Console.WriteLine($"Running {command} {args}...");
            
            var psi = new ProcessStartInfo(command, args)
            {
                WorkingDirectory = workingDirectory ?? Environment.CurrentDirectory,
                CreateNoWindow = true,
                
                RedirectStandardOutput = true,
                RedirectStandardError = true,
                
                UseShellExecute = false,
                WindowStyle = ProcessWindowStyle.Hidden
            };

            var p = Process.Start(psi);
            if (p == null) return;

            var output = p.StandardOutput.ReadToEnd() + p.StandardError.ReadToEnd();

            p.WaitForExit();
            
            if (p.ExitCode == 0) return;

            Console.WriteLine(output);
            Console.WriteLine($"Command {command} {args} failed!");
        }
    }

    public class GitHubRelease
    {
        [JsonProperty(@"id")]
        public int Id;

        [JsonProperty(@"tag_name")]
        public string TagName => $"{Name}";

        [JsonProperty(@"name")]
        public string Name;

        [JsonProperty(@"draft")]
        public bool Draft;

        [JsonProperty(@"prerelease")]
        public bool PreRelease;

        [JsonProperty(@"upload_url")]
        public string UploadUrl;

        [JsonProperty(@"body")]
        public string Body;
    }
}