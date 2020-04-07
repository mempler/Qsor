using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using osu.Framework.Platform;

namespace Qsor.Deploy
{
    internal static class Program
    {
        private static string NugetPackages => Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile), @".nuget\packages");
        private static string NugetPath => Path.Combine(NugetPackages, @"nuget.commandline\5.4.0\tools\NuGet.exe");
        private static string SquirrelPath => Path.Combine(NugetPackages, @"ppy.squirrel.windows\1.9.0.4\tools\Squirrel.exe");
        
        private static void Main(string[] args)
        {
            var currentDirectory = new NativeStorage(".");
            var solutionDirectory = new NativeStorage("..");
            
            currentDirectory.DeleteDirectory("./staging");

            if (!Directory.Exists("releases"))
                Directory.CreateDirectory("releases");

            var stagingDirectory = currentDirectory.GetStorageForDirectory("staging");
            var currentDate = DateTime.Now.ToString("yyyy.MMdd.");
            
            currentDate = currentDate.Replace(".0", ".");
            currentDate += currentDirectory.GetDirectories("releases").Count(s => s.Contains(currentDate));

            var releaseDirectory = currentDirectory.GetStorageForDirectory($"./releases/App-{currentDate}");

            Console.WriteLine($"Package: Qsor");
            Console.WriteLine($"Release Version: {currentDate}");
            Console.WriteLine($"Release Directory: {releaseDirectory.GetFullPath(".")}");

            var logo = solutionDirectory.GetFullPath("Qsor.Game/Resources/Textures/Logo-256x256.png");
            var icon = solutionDirectory.GetFullPath("Qsor.Desktop/icon.ico");
            
            RunCommand("dotnet", $"publish -f netcoreapp3.1 Qsor.Desktop --configuration Release --runtime win-x64 -p:Version={currentDate} -o {stagingDirectory.GetFullPath(".")}",
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
        }

        private static bool RunCommand(string command, string args, string workingDirectory = null)
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
            if (p == null)
                return false;

            var output = p.StandardOutput.ReadToEnd() + p.StandardError.ReadToEnd();

            p.WaitForExit();
            
            if (p.ExitCode == 0)
                return true;
            
            Console.WriteLine(output);
            Console.WriteLine($"Command {command} {args} failed!");

            return false;
        }
    }
}