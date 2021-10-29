using osu.Framework.Graphics.Cursor;
using osu.Framework.Graphics.Sprites;
using osu.Framework.Input.Events;
using osu.Framework.Localisation;

namespace Qsor.Game.Graphics
{
    public class ClickableIcon : SpriteIcon, IHasTooltip
    {
        public LocalisableString TooltipText { get; set; }

        public delegate bool ClickDelegate(ClickEvent e);
        public ClickDelegate ClickEvent;
        
        protected override bool OnClick(ClickEvent e)
        {
            return ClickEvent?.Invoke(e) ?? false;
        }
    }
}