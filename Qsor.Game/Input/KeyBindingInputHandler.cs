using System.Collections.Generic;
using System.Linq;
using osu.Framework.Graphics;
using osu.Framework.Input;
using osu.Framework.Input.Bindings;

namespace Qsor.Game.Input
{
    public class GlobalKeyBindingInputHandler : KeyBindingContainer<GlobalAction>, IHandleGlobalKeyboardInput
    {
        private readonly Drawable _handler;

        public GlobalKeyBindingInputHandler(QsorBaseGame game)
            : base(matchingMode: KeyCombinationMatchingMode.Modifiers)
        {
            _handler = game;
        }
        
        public override IEnumerable<KeyBinding> DefaultKeyBindings => new[]
        {
            new KeyBinding(new [] { InputKey.Control, InputKey.O }, GlobalAction.ToggleOptions),
            new KeyBinding(InputKey.Escape, GlobalAction.ExitOverlay), 
        };
        
        protected override IEnumerable<Drawable> KeyBindingInputQueue =>
            _handler == null ? base.KeyBindingInputQueue : base.KeyBindingInputQueue.Prepend(_handler);
    }

    public enum GlobalAction
    {
        ToggleOptions,
        ExitOverlay
    }
}