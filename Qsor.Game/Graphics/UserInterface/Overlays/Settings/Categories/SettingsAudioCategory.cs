﻿using osu.Framework.Graphics.Sprites;

namespace Qsor.Game.Graphics.UserInterface.Overlays.Settings.Categories
{
    public partial class SettingsAudioCategory : SettingsCategoryContainer
    {
        public override string Name => "Audio";
        public override IconUsage Icon => FontAwesome.Solid.AudioDescription;
    }
}