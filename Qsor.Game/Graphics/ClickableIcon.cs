using osu.Framework.Graphics.Sprites;
using osu.Framework.Input.Events;

namespace Qsor.Game.Graphics
{
    public class ClickableIcon : SpriteIcon
    {
        public delegate bool ClickDelegate(ClickEvent e);
        public ClickDelegate ClickEvent;
        
        protected override bool OnClick(ClickEvent e)
        {
            return ClickEvent?.Invoke(e) ?? false;
        }
    }
}