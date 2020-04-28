using System;

namespace Qsor.Game.Beatmaps
{
    public class BeatmapException : Exception
    {
        public BeatmapException(string msg) : base(msg)
        {
        }
    }
}