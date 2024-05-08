using osu.Framework.Allocation;
using osu.Framework.Graphics.Sprites;
using Qsor.Game.Graphics.UserInterface.Overlays.Settings.Drawables;
using Qsor.Game.Graphics.UserInterface.Overlays.Settings.Drawables.Objects;

namespace Qsor.Game.Graphics.UserInterface.Overlays.Settings.Categories
{
    public partial class SettingsGeneralCategory : SettingsCategoryContainer
    {
        public override string Name => "General";
        public override IconUsage Icon => FontAwesome.Solid.Cog;
        
        [BackgroundDependencyLoader]
        private void Load()
        {
            var signIn = new DrawableSettingsSubCategory("SIGN IN");
            signIn.Content.Add(new DrawableSettingsInput("", "Username", "Enter your username"));
            signIn.Content.Add(new DrawableSettingsInput("false", "Password", "Enter your password", true));
            signIn.Content.Add(new DrawableSettingsCheckbox(true, "Remember Username", "Remember the username next time Qsor starts."));
            signIn.Content.Add(new DrawableSettingsCheckbox(false, "Remember Password", "Remember the password next time Qsor starts."));
            
            AddInternal(signIn);
        }
    }
}