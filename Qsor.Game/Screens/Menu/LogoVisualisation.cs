// Copyright (c) ppy Pty Ltd <contact@ppy.sh>. Licensed under the MIT Licence.
// See the LICENCE file in the repository root for full licence text.

using System;
using System.Linq;
using osu.Framework.Allocation;
using osu.Framework.Bindables;
using osu.Framework.Extensions.Color4Extensions;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Batches;
using osu.Framework.Graphics.OpenGL.Vertices;
using osu.Framework.Graphics.Primitives;
using osu.Framework.Graphics.Shaders;
using osu.Framework.Graphics.Textures;
using osu.Framework.Utils;
using osuTK;
using osuTK.Graphics;
using Qsor.Beatmaps;

namespace Qsor.Screens.Menu
{
    public class LogoVisualisation : Drawable
    {
         private readonly IBindable<WorkingBeatmap> _beatmap = new Bindable<WorkingBeatmap>();

        /// <summary>
        /// The number of bars to jump each update iteration.
        /// </summary>
        private const int IndexChange = 5;

        /// <summary>
        /// The maximum length of each bar in the visualiser. Will be reduced when kiai is not activated.
        /// </summary>
        private const float BarLength = 600;

        /// <summary>
        /// The number of bars in one rotation of the visualiser.
        /// </summary>
        private const int BarsPerVisualiser = 200;

        /// <summary>
        /// How many times we should stretch around the circumference (overlapping overselves).
        /// </summary>
        private const float VisualiserRounds = 5;

        /// <summary>
        /// How much should each bar go down each millisecond (based on a full bar).
        /// </summary>
        private const float DecayPerMilisecond = 0.0024f;

        /// <summary>
        /// Number of milliseconds between each amplitude update.
        /// </summary>
        private const float TimeBetweenUpdates = 50;

        /// <summary>
        /// The minimum amplitude to show a bar.
        /// </summary>
        private const float AmplitudeDeadZone = 1f / BarLength;

        private int _indexOffset;

        public Color4 AccentColour { get; set; }

        private readonly float[] _frequencyAmplitudes = new float[256];

        private IShader _shader;
        private readonly Texture _texture;

        public LogoVisualisation()
        {
            _texture = Texture.WhitePixel;
            Blending = BlendingParameters.Additive;
        }

        [BackgroundDependencyLoader]
        private void Load(ShaderManager shaders, BeatmapManager beatmapManager)
        {
            _beatmap.BindTo(beatmapManager.WorkingBeatmap);
            _shader = shaders.Load(VertexShaderDescriptor.TEXTURE_2, FragmentShaderDescriptor.TEXTURE_ROUNDED);
            
            UpdateColour();
        }

        private void UpdateAmplitudes()
        {
            var track = _beatmap.Value?.Track?.IsLoaded == true ? _beatmap.Value.Track : null;
            var timingPoint = _beatmap.Value?.GetTimingPointAt(Time.Current);
            
            var temporalAmplitudes = track?.CurrentAmplitudes.FrequencyAmplitudes;

            for (var i = 0; i < BarsPerVisualiser; i++)
            {
                if (track?.IsRunning ?? false)
                {
                    var targetAmplitude = (temporalAmplitudes?[(i + _indexOffset) % BarsPerVisualiser] ?? 0) * (timingPoint?.KiaiMode == true ? 1 : 0.5f);
                    if (targetAmplitude > _frequencyAmplitudes[i])
                        _frequencyAmplitudes[i] = targetAmplitude;
                }
                else
                {
                    var index = (i + IndexChange) % BarsPerVisualiser;
                    if (_frequencyAmplitudes[index] > _frequencyAmplitudes[i])
                        _frequencyAmplitudes[i] = _frequencyAmplitudes[index];
                }
            }

            _indexOffset = (_indexOffset + IndexChange) % BarsPerVisualiser;
        }

        private void UpdateColour()
        {
            var defaultColour = Color4.White.Opacity(0.2f);
            
            AccentColour = defaultColour;
        }

        protected override void LoadComplete()
        {
            base.LoadComplete();

            var delayed = Scheduler.AddDelayed(UpdateAmplitudes, TimeBetweenUpdates, true);
            delayed.PerformRepeatCatchUpExecutions = false;
        }

        protected override void Update()
        {
            base.Update();

            var decayFactor = (float)Time.Elapsed * DecayPerMilisecond;

            for (var i = 0; i < BarsPerVisualiser; i++)
            {
                //3% of extra bar length to make it a little faster when bar is almost at it's minimum
                _frequencyAmplitudes[i] -= decayFactor * (_frequencyAmplitudes[i] + 0.03f);
                if (_frequencyAmplitudes[i] < 0)
                    _frequencyAmplitudes[i] = 0;
            }

            Invalidate(Invalidation.DrawNode);
        }

        protected override DrawNode CreateDrawNode() => new VisualisationDrawNode(this);

        private class VisualisationDrawNode : DrawNode
        {
            protected new LogoVisualisation Source => (LogoVisualisation)base.Source;

            private IShader _shader;
            private Texture _texture;

            //Assuming the logo is a circle, we don't need a second dimension.
            private float _size;

            private Color4 _colour;
            private float[] _audioData;

            private readonly QuadBatch<TexturedVertex2D> _vertexBatch = new QuadBatch<TexturedVertex2D>(100, 10);

            public VisualisationDrawNode(LogoVisualisation source)
                : base(source)
            {
            }

            public override void ApplyState()
            {
                base.ApplyState();

                _shader = Source._shader;
                _texture = Source._texture;
                _size = Source.DrawSize.X;
                _colour = Source.AccentColour;
                _audioData = Source._frequencyAmplitudes;
            }

            public override void Draw(Action<TexturedVertex2D> vertexAction)
            {
                base.Draw(vertexAction);

                _shader.Bind();

                var inflation = DrawInfo.MatrixInverse.ExtractScale().Xy;

                var colourInfo = DrawColourInfo.Colour;
                colourInfo.ApplyChild(_colour);

                if (_audioData != null)
                {
                    for (var j = 0; j < VisualiserRounds; j++)
                    {
                        for (var i = 0; i < BarsPerVisualiser; i++)
                        {
                            if (_audioData[i] < AmplitudeDeadZone)
                                continue;

                            var rotation = MathUtils.DegreesToRadians(i / (float)BarsPerVisualiser * 360 + j * 360 / VisualiserRounds);
                            var rotationCos = MathF.Cos(rotation);
                            var rotationSin = MathF.Sin(rotation);
                            //taking the cos and sin to the 0..1 range
                            var barPosition = new Vector2(rotationCos / 2 + 0.5f, rotationSin / 2 + 0.5f) * _size;

                            var barSize = new Vector2(_size * MathF.Sqrt(2 * (1 - MathF.Cos(MathUtils.DegreesToRadians(360f / BarsPerVisualiser)))) / 2f, BarLength * _audioData[i]);
                            //The distance between the position and the sides of the bar.
                            var bottomOffset = new Vector2(-rotationSin * barSize.X / 2, rotationCos * barSize.X / 2);
                            //The distance between the bottom side of the bar and the top side.
                            var amplitudeOffset = new Vector2(rotationCos * barSize.Y, rotationSin * barSize.Y);

                            var rectangle = new Quad(
                                Vector2Extensions.Transform(barPosition - bottomOffset, DrawInfo.Matrix),
                                Vector2Extensions.Transform(barPosition - bottomOffset + amplitudeOffset, DrawInfo.Matrix),
                                Vector2Extensions.Transform(barPosition + bottomOffset, DrawInfo.Matrix),
                                Vector2Extensions.Transform(barPosition + bottomOffset + amplitudeOffset, DrawInfo.Matrix)
                            );

                            DrawQuad(
                                _texture,
                                rectangle,
                                colourInfo,
                                null,
                                _vertexBatch.AddAction,
                                //barSize by itself will make it smooth more in the X axis than in the Y axis, this reverts that.
                                Vector2.Divide(inflation, barSize.Yx));
                        }
                    }
                }

                _shader.Unbind();
            }

            protected override void Dispose(bool isDisposing)
            {
                base.Dispose(isDisposing);

                _vertexBatch.Dispose();
            }
        }
    }
}