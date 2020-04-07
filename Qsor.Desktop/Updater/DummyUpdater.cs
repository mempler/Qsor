using osu.Framework.Allocation;
using Qsor.Game.Updater;

namespace Qsor.Desktop.Updater
{
   [Cached]
   public class DummyUpdater : Game.Updater.Updater
   {
      public override void CheckAvailable()
      {
         BindableStatus.Value = UpdaterStatus.Latest;
      }

      public override void UpdateGame()
      {
         // It's a dummy updater! It doesn't do anything!
      }
   }
}