using System;

namespace Result
{
    public class SpecialInfo
    {
        public string _text;
        public float _bonusRate;

        public SpecialInfo() {
            _text = "";
            _bonusRate = 1f;
        }

        public static SpecialInfo TrafficLight()
        {
            var info = new SpecialInfo();
            info._bonusRate = 1.2f;
            info._text = $"信号機×{info._bonusRate}";
            return info;
        }

        public static SpecialInfo RGB()
        {
            var info = new SpecialInfo();
            info._bonusRate = 1.25f;
            info._text = $"RGB×{info._bonusRate}";
            return info;
        }

        public static SpecialInfo Monochrome()
        {
            var info = new SpecialInfo();
            info._bonusRate = 1.25f;
            info._text = $"モノクロ×{info._bonusRate}";
            return info;
        }

        public static SpecialInfo MonochromeAll()
        {
            var info = new SpecialInfo();
            info._bonusRate = 2.0f;
            info._text = $"モノクロALL×{info._bonusRate}";
            return info;
        }

        public static SpecialInfo Rainbow()
        {
            var info = new SpecialInfo();
            info._bonusRate = 1.5f;
            info._text = $"虹色×{info._bonusRate}";
            return info;
        }

        public static SpecialInfo RainbowAll()
        {
            var info = new SpecialInfo();
            info._bonusRate = 2.5f;
            info._text = $"虹色ALL×{info._bonusRate}";
            return info;
        }

        public static SpecialInfo Straight(int num)
        {
            var info = new SpecialInfo();
            info._bonusRate = 1 + (0.2f) * num;
            info._text = $"ストレート{num}×{info._bonusRate}";
            return info;
        }

        public static SpecialInfo BigToro3Kan()
        {
            var info = new SpecialInfo();
            info._bonusRate = 1.5f;
            info._text = $"大トロ３貫握り×{info._bonusRate}";
            return info;
        }

        public static SpecialInfo MidiumToro3Kan()
        {
            var info = new SpecialInfo();
            info._bonusRate = 1.4f;
            info._text = $"中トロ３貫握り×{info._bonusRate}";
            return info;
        }

        public static SpecialInfo SmallToro3Kan()
        {
            var info = new SpecialInfo();
            info._bonusRate = 1.3f;
            info._text = $"小トロ３貫握り×{info._bonusRate}";
            return info;
        }

        internal static SpecialInfo MunielRush()
        {
            var info = new SpecialInfo();
            info._bonusRate = 1.3f;
            info._text = $"ムニエルづくし×{info._bonusRate}";
            return info;
        }

        internal static SpecialInfo Tataki3Kind()
        {
            var info = new SpecialInfo();
            info._bonusRate = 1.5f;
            info._text = $"たたき３種盛り×{info._bonusRate}";
            return info;
        }

        internal static SpecialInfo TunaRush()
        {
            var info = new SpecialInfo();
            info._bonusRate = 1.3f;
            info._text = $"ツナ缶パック×{info._bonusRate}";
            return info;
        }
    }
}