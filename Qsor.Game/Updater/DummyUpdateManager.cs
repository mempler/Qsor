﻿using osu.Framework.Allocation;

namespace Qsor.Game.Updater
{
   [Cached]
   public partial class DummyUpdater : UpdateManager
   {
      public override void CheckAvailable()
      {
         BindableStatus.Value = UpdaterStatus.Latest;
      }

      public override void UpdateGame()
      {
      }
   }
}
