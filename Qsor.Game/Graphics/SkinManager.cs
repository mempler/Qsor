using osu.Framework.Allocation;
using osu.Framework.Bindables;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Colour;

namespace Qsor.Game.Graphics
{
    public class SkinManager : Component
    {
        public BindableList<ColourInfo> BindableSkinColours = new BindableList<ColourInfo>();

        [BackgroundDependencyLoader]
        private void Load()
        {
            
        }
    }
}