using System;
using System.Diagnostics;
using osu.Framework;
using osu.Framework.Development;
using Qsor.Desktop.Updater;
using Qsor.Game;
using Qsor.Game.Updater;

namespace Qsor.Desktop;

public partial class QsorGameDesktop : QsorGame
{
    public QsorGameDesktop(string[] args = null)
        : base(args)
    {
    }

    protected override Game.Updater.UpdateManager CreateUpdater()
    {
        if (DebugUtils.IsDebugBuild)
            return new DummyUpdater();
    
        switch (RuntimeInfo.OS)
        {
            case RuntimeInfo.Platform.Windows:
                Debug.Assert(OperatingSystem.IsWindows()); // To silence the "only supported on Windows" warning

                return new SquirrelUpdateManager();

            default:
                return new DummyUpdater();
        }
    }
}
