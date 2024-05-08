using osu.Framework.Allocation;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Sprites;
using osuTK;
using Qsor.Game.Beatmaps;
using Qsor.Game.Graphics.UserInterface.Overlays.Notification;

namespace Qsor.Game.Graphics.UserInterface.Overlays
{
    public partial class MusicPlayerOverlay : CompositeDrawable
    {
        private FillFlowContainer<Drawable> _flow;
        private FillFlowContainer<ClickableSpriteIcon> _icons;

        private ClickableSpriteIcon _skipBackButton;
        private ClickableSpriteIcon _playButton;
        private ClickableSpriteIcon _pauseButton;
        private ClickableSpriteIcon _stopButton;
        private ClickableSpriteIcon _skipButton;
        private ClickableSpriteIcon _infoButton;
        private ClickableSpriteIcon _collectionButton;

        private DragableProgressbar _progressbar;

        private BeatmapManager _beatmapManager;

        [BackgroundDependencyLoader]
        private void Load(NotificationOverlay notificationOverlay, BeatmapManager beatmapManager)
        {
            _beatmapManager = beatmapManager;
            
            AutoSizeAxes = Axes.Both;
            Padding = new MarginPadding(8);
            
            _flow = new FillFlowContainer
            {
                AutoSizeAxes = Axes.Both,
                Direction = FillDirection.Vertical,
                Spacing = new Vector2(6),
            };
            
            _icons = new FillFlowContainer<ClickableSpriteIcon>
            {
                AutoSizeAxes = Axes.Both,
                Direction = FillDirection.Horizontal,
                Spacing = new Vector2(16, 0),
            };
            
            _icons.Add(_skipBackButton = new ClickableSpriteIcon
            {
                Icon = FontAwesome.Solid.StepBackward,
                Size = new Vector2(16),
                
                TooltipText = "Previous track",
                
                ClickEvent = e =>
                {
                    notificationOverlay.AddBigNotification("<< Prev", 1100);
                    
                    beatmapManager?.PreviousRandomMap();
                    beatmapManager?.WorkingBeatmap?.Value?.Play();
                    
                    return true;
                }
            });
            
            _icons.Add(_playButton = new ClickableSpriteIcon
            {
                Icon = FontAwesome.Solid.Play,
                Size = new Vector2(16),
                
                TooltipText = "Play",
                
                ClickEvent = _ =>
                {
                    notificationOverlay.AddBigNotification("Play", 1100);

                    var track = beatmapManager?.WorkingBeatmap?.Value?.Track;
                    if (track != null)
                    {
                        if (track.IsRunning)
                        {
                            track.Restart();
                        }
                        else
                        {
                            track.Start();
                        }
                    }

                    return true;
                }
            });
            
            _icons.Add(_pauseButton = new ClickableSpriteIcon
            {
                Icon = FontAwesome.Solid.Pause,
                Size = new Vector2(16),
                
                TooltipText = "Pause",
                
                ClickEvent = e =>
                {
                    beatmapManager?.WorkingBeatmap?.Value?.Track?.Start();
                    
                    var track = beatmapManager?.WorkingBeatmap?.Value?.Track;
                    if (track != null)
                    {
                        if (track.IsRunning)
                        {
                            notificationOverlay.AddBigNotification("Pause", 1100);
                            track.Stop();
                        }
                        else
                        {
                            notificationOverlay.AddBigNotification("Unpause", 1100);
                            track.Start();
                        }
                    }
                    else
                    {
                        notificationOverlay.AddBigNotification("Pause", 1100);
                    }

                    return true;
                }
            });
            
            _icons.Add(_stopButton = new ClickableSpriteIcon
            {
                Icon = FontAwesome.Solid.Stop,
                Size = new Vector2(16),
                
                TooltipText = "Stop the music!",
                
                ClickEvent = e =>
                {
                    notificationOverlay.AddBigNotification("Stop Playing", 1100);
                    
                    var track = beatmapManager?.WorkingBeatmap?.Value?.Track;
                    track?.Stop();
                    track?.Reset();
                    return true;
                }
            });
            
            _icons.Add(_skipButton = new ClickableSpriteIcon
            {
                Icon = FontAwesome.Solid.StepForward,
                Size = new Vector2(16),
                
                TooltipText = "Next track",
                
                ClickEvent = e =>
                {
                    notificationOverlay.AddBigNotification(">> Next", 1100);
                    
                    beatmapManager?.NextRandomMap();
                    beatmapManager?.WorkingBeatmap?.Value?.Play();
                    
                    return true;
                }
            });
            
            _icons.Add(_infoButton = new ClickableSpriteIcon
            {
                Icon = FontAwesome.Solid.Info,
                Size = new Vector2(16),
                
                TooltipText = "View song info",
                
                ClickEvent = e =>
                {
                    notificationOverlay.AddBigNotification("Song info will be permanently displayed.", 1100);
                    return true;
                }
            });
            
            _icons.Add(_collectionButton = new ClickableSpriteIcon
            {
                Icon = FontAwesome.Solid.Bars,
                Size = new Vector2(16),
                
                TooltipText = "Jump To window",
                
                ClickEvent = e =>
                {
                    notificationOverlay.AddBigNotification("!Unimplemented!", 1100);
                    return true;
                }
            });
            
            _flow.Add(_icons);
            
            _flow.Add(_progressbar = new DragableProgressbar
            {
                Position = new Vector2(0, 24),
                
                TooltipText = "Drag to seek to a specific point in the song.",
            });
            
            _progressbar.Current.ValueChanged += e =>
            {
                var track = _beatmapManager?.WorkingBeatmap?.Value?.Track;
                if (track != null && (_progressbar.IsDragging || _progressbar.JustClicked))
                {
                    track.Seek(e.NewValue * track.Length);
                    _progressbar.JustClicked = false;
                }
            };
            
            AddInternal(_flow);
        }

        protected override void Update()
        {
            var track = _beatmapManager?.WorkingBeatmap?.Value?.Track;
            if (track != null && !_progressbar.IsDragging)
            {
                _progressbar.Current.Value = track.CurrentTime / track.Length;
            }
        }
    }
}