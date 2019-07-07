using UnityEngine;

namespace Target
{
    public enum ColorType
    {
        Blue,
        Yellow,
        Red,
        Green,
        Orange,
        Indigo,
        Violet,
        White,
        Gray,
        Black,
    }

    public static partial class EnumExtend
    {
        static Color ColorOrange = new Color(1f, 0.5f, 0f);
        static Color ColorIndigo = new Color(0.3f, 0f, 0.55f);
        static Color ColorViolet = new Color(0.58f, 0f, 0.83f);

        public static Color GetColor(this ColorType type)
        {
            switch (type)
            {
                case ColorType.Blue: return Color.blue;
                case ColorType.Yellow: return Color.yellow;
                case ColorType.Red: return Color.red;
                case ColorType.Green: return Color.green;
                case ColorType.Orange: return ColorOrange;
                case ColorType.Indigo: return ColorIndigo;
                case ColorType.Violet: return ColorViolet;
                case ColorType.White: return Color.white;
                case ColorType.Gray: return Color.gray;
                case ColorType.Black: return Color.black;
            }
            Debug.Assert(false, "Should not reach here");
            return Color.magenta;
        }
    }
}