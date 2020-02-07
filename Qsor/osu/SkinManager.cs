using osuTK.Graphics;

namespace Qsor.osu
{
    public enum SkinElementType
    {
        Skinnable
    }
    
    public static class SkinManager
    {
        public static Color4[] SkinColors =
        {
            Color4.Red,
            Color4.Blue,
            Color4.Yellow,
            Color4.Green
        };

        private static string _skinName;
        public static string SkinName
        {
            get => _skinName;
            set
            {
                _skinName = value;
            }
        }
    }
}