using osu.Framework.Graphics.Cursor;
using osu.Framework.Input.Events;
using osu.Framework.Localisation;
using Qsor.Game.Graphics.Drawables;

namespace Qsor.Game.Graphics
{
    public partial class DragableProgressbar : DrawableProgressbar, IHasTooltip
    {
        public LocalisableString TooltipText { get; set; }

        protected override bool OnClick(ClickEvent e)
        {
            Current.Value = e.MousePosition.X / DrawWidth;
            return true;
        }

        protected override bool OnDragStart(DragStartEvent e) => true;

        protected override void OnDrag(DragEvent e)
        {
            Current.Value = e.MousePosition.X / DrawWidth;
        }
    }
}