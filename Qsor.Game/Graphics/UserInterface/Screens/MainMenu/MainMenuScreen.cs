﻿using osu.Framework.Allocation;
using osu.Framework.Audio;
using osu.Framework.Bindables;
using osu.Framework.Development;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Input.Events;
using osu.Framework.Localisation;
using osu.Framework.Platform;
using osu.Framework.Screens;
using osu.Framework.Timing;
using osuTK;
using osuTK.Graphics;
using Qsor.Game.Beatmaps;
using Qsor.Game.Database;
using Qsor.Game.Graphics.Containers;
using Qsor.Game.Graphics.UserInterface.Overlays;
using Qsor.Game.Graphics.UserInterface.Overlays.Notification;
using Qsor.Game.Graphics.UserInterface.Screens.MainMenu.Containers;
using Qsor.Game.Graphics.UserInterface.Screens.MainMenu.Drawables;

namespace Qsor.Game.Graphics.UserInterface.Screens.MainMenu
{
    public partial class MainMenuScreen : Screen
    {
        private BackgroundImageContainer _background;
        private DrawableQsorLogo _drawableQsorLogo;
        private Toolbar _toolbar;
        private BottomBar _bottomBar;
        private DrawableMenuSideFlashes _sideFlashes;

        private Bindable<WorkingBeatmap> _workingBeatmap = new();
        
        [Resolved]
        private NotificationOverlay NotificationOverlay { get; set; }
        
        [Resolved]
        private Storage Storage { get; set; }
        
        [Resolved]
        private Updater.UpdateManager Updater { get; set; }

        [Resolved] 
        private GameHost Host { get; set; }
        
        [BackgroundDependencyLoader]
        private void Load(UpdaterOverlay updaterOverlay, AudioManager audioManager, QsorDbContextFactory ctxFactory, BeatmapManager beatmapManager)
        {
            var parallaxBack = new ParallaxContainer
            {
                ParallaxAmount = -0.005f
            };
            parallaxBack._content.Add(_background = new BackgroundImageContainer
            {
                Anchor = Anchor.Centre,
                Origin = Anchor.Centre,
                FillMode = FillMode.Fill,
            });

            _workingBeatmap.BindTo(beatmapManager.WorkingBeatmap);
            _workingBeatmap.ValueChanged += e =>
            {
                if (e.NewValue == null)
                    return;
                
                LoadComponent(e.NewValue);
            
                _background.SetTexture(e.NewValue.Background);
                audioManager.AddItem(e.NewValue.Track);
            };
            
            var parallaxFront = new ParallaxContainer
            {
                ParallaxAmount = -0.015f,
                RelativeSizeAxes = Axes.Both,
            };
            parallaxFront._content.Add(new DrawSizePreservingFillContainer
            {
                Child = _drawableQsorLogo = new DrawableQsorLogo
                {
                    Anchor = Anchor.Centre,
                    Origin = Anchor.Centre,
                    AutoSizeAxes = Axes.Both,
                    Scale = new Vector2(1f) * (DrawSize.X / DrawSize.Y)
                }
            });

            InternalChildren = new Drawable[]
            {
                parallaxBack,
                parallaxFront,

                _sideFlashes = new DrawableMenuSideFlashes(),
                _toolbar = new Toolbar(),
                _bottomBar = new BottomBar(),

                updaterOverlay
            };

            // Start a random map, TODO: remove this from here
            beatmapManager.NextRandomMap();
            beatmapManager.WorkingBeatmap.Value?.Play();
        }
        
        public override void OnEntering(ScreenTransitionEvent e)
        {
            _clock.Start();
            
            this.FadeInFromZero(2500, Easing.InExpo).Finally(e =>
            {
                NotificationOverlay.AddNotification(new LocalisableString(
                        "Please note that this game is in very early development and is not representative of the final product. " +
                        "If you encounter any issues, please report them on our GitHub repository!"),
                    Color4.Orange, 10000);

                NotificationOverlay.AddNotification(new LocalisableString(
                        "You can play different beatmaps by editing \"game.ini\" config file. " +
                        "To open the Qsor configuration directory, click this notification!"),
                    Color4.Orange, 10000, () => { Storage.OpenFileExternally(string.Empty); });

                NotificationOverlay.AddNotification(new LocalisableString(
                        $"You're currently running {QsorBaseGame.Version}! " +
                        "Click here to view the changelog."),
                    Color4.Gray, 10000, () => Host.OpenUrlExternally($"https://github.com/osuAkatsuki/Qsor/releases/tag/{QsorBaseGame.Version}"));

                if (!DebugUtils.IsDebugBuild)
                    Updater.CheckAvailable();
            });
        }

        public override bool OnExiting(ScreenExitEvent e)
        {
            this.FadeOutFromOne(2500, Easing.OutExpo);
            return true;
        }
        
        // Fade clock
        private StopwatchClock _clock = new();
        private bool _isFading;
        
        protected override void Update()
        {
            if (_isFading || _clock.ElapsedMilliseconds <= 5000)
                return;
            
            _toolbar.FadeOut(8000);
            _bottomBar.FadeOut(8000);
            
            _isFading = true;
        }
        protected override bool OnMouseMove(MouseMoveEvent e)
        {
            if (_clock.ElapsedMilliseconds >= 5000)
            {
                _toolbar.ClearTransforms();
                _bottomBar.ClearTransforms();

                _toolbar.FadeIn(250);
                _bottomBar.FadeIn(250);
                
                _isFading = false;
                _clock.Restart();
            }
            
            return true;
        }

        protected override bool OnClick(ClickEvent e)
        {
            if (_drawableQsorLogo.IsHovered)
            {
                NotificationOverlay.AddBigNotification("Gameplay as it stands right now is not implemented.\n" +
                                                    "We want to implement the UI First before we do any gameplay features.\n" +
                                                    "That way we can guarantee that it feels just right!", 10000);
            }

            return base.OnClick(e);
        }
    }
}