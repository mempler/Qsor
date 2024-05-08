﻿using osu.Framework.Graphics.Sprites;

namespace Qsor.Game.Graphics.UserInterface.Overlays.Settings.Categories
{
    public partial class SettingsEditorCategory : SettingsCategoryContainer
    {
        public override string Name => "Maintenance";
        public override IconUsage Icon => FontAwesome.Solid.PencilAlt;
    }
}