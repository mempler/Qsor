using System.Linq;
using osu.Framework.Graphics.Cursor;
using osu.Framework.Input.Events;
using osu.Framework.Localisation;
using osuTK.Input;
using Qsor.Game.Graphics.Drawables;

namespace Qsor.Game.Graphics
{
    public class DragableProgressbar : DrawableProgressbar, IHasTooltip
    {
        public LocalisableString TooltipText { get; set; }
        
        public bool IsDragging { get; private set; }
        public bool JustClicked { get; set; } // TODO: use an action
        
        protected override bool OnClick(ClickEvent e)
        {
            Current.Value = e.MousePosition.X / DrawWidth;
            JustClicked = true;
            return true;
        }

        protected override bool OnMouseMove(MouseMoveEvent e)
        {
            // void OnDrag(DragEvent e) does not work atm
            if (e.PressedButtons.Contains(MouseButton.Left)
            ||  e.PressedButtons.Contains(MouseButton.Right))
            {
                Current.Value = e.MousePosition.X / DrawWidth;

                IsDragging = true;
            }
            else
            {
                IsDragging = false;
            }
            
            return base.OnMouseMove(e);
        }
    }
}