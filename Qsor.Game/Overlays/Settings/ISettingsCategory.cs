using osu.Framework.Graphics.Sprites;

namespace Qsor.Game.Overlays.Settings
{
    public interface ISettingsCategory
    {
        string Name { get; }
        IconUsage Icon { get; }
    }
}