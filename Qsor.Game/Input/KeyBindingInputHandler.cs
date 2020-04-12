using System.Collections.Generic;
using System.Linq;
using osu.Framework.Allocation;
using osu.Framework.Graphics;
using osu.Framework.Input;
using osu.Framework.Input.Bindings;
using Qsor.Game.Overlays;

namespace Qsor.Game.Input
{
    public class GlobalKeyBindingInputHandler : KeyBindingContainer<GlobalAction>, IHandleGlobalKeyboardInput
    {
        private readonly Drawable _handler;

        public GlobalKeyBindingInputHandler(QsorBaseGame game)
            : base(matchingMode: KeyCombinationMatchingMode.Modifiers)
        {
            if (game is IKeyBindingHandler<GlobalAction>)
                _handler = game;
        }
        
        public override IEnumerable<KeyBinding> DefaultKeyBindings => new[]
        {

            new KeyBinding(new [] { InputKey.Control, InputKey.O }, GlobalAction.ToggleOptions), 

        };
        
        protected override IEnumerable<Drawable> KeyBindingInputQueue =>
            _handler == null ? base.KeyBindingInputQueue : base.KeyBindingInputQueue.Prepend(_handler);
    }

    public enum GlobalAction
    {
        ToggleOptions
    }
}