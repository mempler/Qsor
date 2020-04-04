using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using osu.Framework.Allocation;
using osu.Framework.Bindables;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Shapes;
using osu.Framework.Graphics.Sprites;
using osu.Framework.Input.Events;
using osu.Framework.Localisation;
using osuTK;
using osuTK.Graphics;

namespace Qsor.Game.Overlays.Settings.Drawables
{
    public class DrawableSettingsDropdown : CompositeDrawable
    {
        public Bindable<DropdownItem> SelectedItem { get; } = new Bindable<DropdownItem>();
        
        private readonly List<DropdownItem> _dropdownItems = new List<DropdownItem>();
        private FillFlowContainer<DropdownItem> _fillFlowContainer;
        
        [BackgroundDependencyLoader]
        private void Load()
        {
            RelativeSizeAxes = Axes.X;
            Height = 24;
            
            AddInternal(_fillFlowContainer = new FillFlowContainer<DropdownItem>
            {
                RelativeSizeAxes = Axes.X,
                AutoSizeAxes = Axes.Y,
                LayoutEasing = Easing.OutBounce,
                LayoutDuration = 500,
            });
            
            _fillFlowContainer.Add(new DropdownItem("test", ""));
        }

        public void Add(DropdownItem item)
        {
            _dropdownItems.Add(item);
        }

        private bool gotClicked;
        protected override bool OnClick(ClickEvent e)
        {
            if (gotClicked)
            {
                var first = _fillFlowContainer.First();
                _fillFlowContainer.Clear(false);
                _fillFlowContainer.Add(first);

                gotClicked = false;
                return true;
            }
            
            foreach (var item in _dropdownItems)
            {
                item.Position = Vector2.Zero;
                _fillFlowContainer.Add(item);
            }
            
            return gotClicked = true;
        }
    }
    
    public class DropdownItem : CompositeDrawable
    {
        public Bindable<string> Key { get; } = new Bindable<string>();
        public Bindable<object> Value { get; } = new Bindable<object>();
        
        public DropdownItem(string key, object value)
        {
            Key.Value = key;
            Value.Value = value;
        }
        
        [BackgroundDependencyLoader]
        private void Load()
        {
            RelativeSizeAxes = Axes.X;
            AutoSizeAxes = Axes.Y;
            CornerRadius = 5;
            Masking = true;
            
            AddInternal(new Box
            {
                RelativeSizeAxes = Axes.X,
                Height = 24,
                Colour = Color4.Black,
                Alpha = .50f
            });
            
            AddInternal(new SpriteText
            {
                Text = new LocalisedString(Key.Value),
                Colour = Color4.White,
                Margin = new MarginPadding { Top = 2, Left = 10 }
            });
        }
    }
}