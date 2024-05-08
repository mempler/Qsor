using osu.Framework.Allocation;

namespace Qsor.Game.Updater
{
   [Cached]
   public partial class DummyUpdater : Updater
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