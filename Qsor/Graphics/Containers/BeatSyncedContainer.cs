// Copyright (c) ppy Pty Ltd <contact@ppy.sh>. Licensed under the MIT Licence.
// See the LICENCE file in the repository root for full licence text.

using System.Linq;
using osu.Framework.Allocation;
using osu.Framework.Audio.Track;
using osu.Framework.Bindables;
using osu.Framework.Graphics.Containers;
using Qsor.Beatmaps;

namespace Qsor.Graphics.Containers
{
    public class BeatSyncedContainer : Container
    {
        protected readonly IBindable<WorkingBeatmap> Beatmap = new Bindable<WorkingBeatmap>();

        private int _lastBeat;
        private TimingPoint _lastTimingPoint;

        /// <summary>
        /// The amount of time before a beat we should fire <see cref="OnNewBeat(int, TimingControlPoint, EffectControlPoint, TrackAmplitudes)"/>.
        /// This allows for adding easing to animations that may be synchronised to the beat.
        /// </summary>
        protected double EarlyActivationMilliseconds;

        /// <summary>
        /// The time in milliseconds until the next beat.
        /// </summary>
        public double TimeUntilNextBeat { get; private set; }

        /// <summary>
        /// The time in milliseconds since the last beat
        /// </summary>
        public double TimeSinceLastBeat { get; private set; }

        /// <summary>
        /// How many beats per beatlength to trigger. Defaults to 1.
        /// </summary>
        public int Divisor { get; set; } = 1;

        /// <summary>
        /// An optional minimum beat length. Any beat length below this will be multiplied by two until valid.
        /// </summary>
        public double MinimumBeatLength { get; set; }

        /// <summary>
        /// Default length of a beat in milliseconds. Used whenever there is no beatmap or track playing.
        /// </summary>
        private const double DefaultBeatLength = 60000.0 / 60.0;

        private TimingPoint _defaultTiming;
        //private EffectControlPoint defaultEffect;
        private TrackAmplitudes _defaultAmplitudes;

        protected bool IsBeatSyncedWithTrack { get; private set; }

        protected override void Update()
        {
            Track track = null;
            Beatmap beatmap = null;

            var currentTrackTime = 0d;
            var timingPoint = new TimingPoint();
            //EffectControlPoint effectPoint = null;

            if (Beatmap.Value.Track.IsLoaded)
            {
                track = Beatmap.Value.Track;
                beatmap = Beatmap.Value;
            }

            if (track != null && beatmap != null && track.IsRunning && track.Length > 0)
            {
                currentTrackTime = track.CurrentTime + EarlyActivationMilliseconds;

                timingPoint = beatmap.TimingPoints.FirstOrDefault(t => t.Offset >= currentTrackTime);
            }

            IsBeatSyncedWithTrack = timingPoint.MsPerBeat > 0;

            if (!IsBeatSyncedWithTrack)
            {
                currentTrackTime = Clock.CurrentTime;
                timingPoint = _defaultTiming;
            }

            var beatLength = timingPoint.MsPerBeat / Divisor;

            while (beatLength < MinimumBeatLength)
                beatLength *= 2;

            var beatIndex = (int)((currentTrackTime - timingPoint.Offset) / beatLength) - (0);

            // The beats before the start of the first control point are off by 1, this should do the trick
            if (currentTrackTime < timingPoint.Offset)
                beatIndex--;

            TimeUntilNextBeat = (timingPoint.Offset - currentTrackTime) % beatLength;
            if (TimeUntilNextBeat < 0)
                TimeUntilNextBeat += beatLength;

            TimeSinceLastBeat = beatLength - TimeUntilNextBeat;

            if (timingPoint.Equals(_lastTimingPoint) && beatIndex == _lastBeat)
                return;

            using (BeginDelayedSequence(-TimeSinceLastBeat, true))
                OnNewBeat(beatIndex, timingPoint, track?.CurrentAmplitudes ?? _defaultAmplitudes);

            _lastBeat = beatIndex;
            _lastTimingPoint = timingPoint;
        }

        [BackgroundDependencyLoader]
        private void Load(BeatmapManager beatmapManager)
        {
            Beatmap.BindTo(beatmapManager.WorkingBeatmap);

            _defaultTiming = new TimingPoint
            {
                MsPerBeat = DefaultBeatLength,
            };
            
            _defaultAmplitudes = new TrackAmplitudes
            {
                FrequencyAmplitudes = new float[256],
                LeftChannel = 0,
                RightChannel = 0
            };
        }

        protected virtual void OnNewBeat(int beatIndex, TimingPoint timingPoint, TrackAmplitudes amplitudes)
        {
        }
    }
}