using System;
using System.Linq;
using LibGit2Sharp;

namespace Qsor.Deploy
{
    public static class ChangelogGenerator
    {
        public static string GenerateChangelog()
        {
            var repoPath = Repository.Discover(".");
            var repo = new Repository(repoPath);

            var changeLog = string.Empty;

            foreach (var commit in repo.Commits)
            {
                var lastTag = repo.Tags.Last();
                if (commit.Sha == lastTag.Target.Sha)
                    break;

                changeLog += $"`{commit.Sha.Remove(7)}` {commit.MessageShort} - {commit.Author.Name} {Environment.NewLine}";
            }
            

            return changeLog;
        }
    }
}