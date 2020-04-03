using osu.Framework.Graphics.Sprites;

namespace Qsor.Game.Overlays.Settings.Categories
{
    public class SettingsEditorCategory : ISettingsCategory
    {
        public string Name => "Editor";
        public IconUsage Icon => FontAwesome.Solid.PencilAlt;
    }
}