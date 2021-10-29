using osu.Framework.Allocation;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Sprites;
using osuTK;
using Qsor.Game.Beatmaps;
using Qsor.Game.Graphics;

namespace Qsor.Game.Overlays
{
    public enum MusicPlayerButtonType
    {
        SkipBack,
        Play,
        Pause,
        Stop,
        Skip,
        Info,
        Collection
    }
    
    public class MusicPlayerOverlay : CompositeDrawable
    {
        private FillFlowContainer<Drawable> _flow;
        private FillFlowContainer<ClickableIcon> _Icons;

        private ClickableIcon SkipBackButton;
        private ClickableIcon PlayButton;
        private ClickableIcon PauseButton;
        private ClickableIcon StopButton;
        private ClickableIcon SkipButton;
        private ClickableIcon InfoButton;
        private ClickableIcon CollectionButton;

        private DragProgressbar Progressbar;

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
                Spacing = new Vector2(6)
            };
            
            _Icons = new FillFlowContainer<ClickableIcon>
            {
                AutoSizeAxes = Axes.Both,
                Direction = FillDirection.Horizontal,
                Spacing = new Vector2(16, 0),
            };
            
            _Icons.Add(SkipBackButton = new ClickableIcon
            {
                Icon = FontAwesome.Solid.StepBackward,
                Size = new Vector2(16),
                
                ClickEvent = e =>
                {
                    notificationOverlay.AddBigNotification("<< Prev !Unimplemented!", 1100);
                    
                    return true;
                }
            });
            
            _Icons.Add(PlayButton = new ClickableIcon
            {
                Icon = FontAwesome.Solid.Play,
                Size = new Vector2(16),
                
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
            
            _Icons.Add(PauseButton = new ClickableIcon
            {
                Icon = FontAwesome.Solid.Pause,
                Size = new Vector2(16),
                
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
            
            _Icons.Add(StopButton = new ClickableIcon
            {
                Icon = FontAwesome.Solid.Stop,
                Size = new Vector2(16),
                
                ClickEvent = e =>
                {
                    notificationOverlay.AddBigNotification("Stop Playing", 1100);
                    
                    var track = beatmapManager?.WorkingBeatmap?.Value?.Track;
                    track?.Stop();
                    track?.Reset();
                    return true;
                }
            });
            
            _Icons.Add(SkipButton = new ClickableIcon
            {
                Icon = FontAwesome.Solid.StepForward,
                Size = new Vector2(16),
                
                ClickEvent = e =>
                {
                    notificationOverlay.AddBigNotification(">> Next !Unimplemented!", 1100);
                    return true;
                }
            });
            
            _Icons.Add(InfoButton = new ClickableIcon
            {
                Icon = FontAwesome.Solid.Info,
                Size = new Vector2(16),
                
                ClickEvent = e =>
                {
                    notificationOverlay.AddBigNotification("Song info will be permanently displayed.", 1100);
                    return true;
                }
            });
            
            _Icons.Add(CollectionButton = new ClickableIcon
            {
                Icon = FontAwesome.Solid.Bars,
                Size = new Vector2(16),
                
                ClickEvent = e =>
                {
                    notificationOverlay.AddBigNotification("!Unimplemented!", 1100);
                    return true;
                }
            });
            
            _flow.Add(_Icons);
            
            _flow.Add(Progressbar = new DragProgressbar
            {
                Position = new Vector2(0, 24)
            });
            
            Progressbar.Current.ValueChanged += e =>
            {
                var track = _beatmapManager?.WorkingBeatmap?.Value?.Track;
                if (track != null && (Progressbar.IsDragging || Progressbar.JustClicked))
                {
                    track.Seek(e.NewValue * track.Length);
                    Progressbar.JustClicked = false;
                }
            };
            
            AddInternal(_flow);
        }

        protected override void Update()
        {
            var track = _beatmapManager?.WorkingBeatmap?.Value?.Track;
            if (track != null && !Progressbar.IsDragging)
            {
                Progressbar.Current.Value = track.CurrentTime / track.Length;
            }
        }
    }
}