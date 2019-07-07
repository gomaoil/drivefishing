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

        static SpecialInfo TrafficLight()
        {
            var info = new SpecialInfo();
            info._bonusRate = 1.1f;
            info._text = $"信号機×{info._bonusRate}";
            return info;
        }

        static SpecialInfo RGB()
        {
            var info = new SpecialInfo();
            info._bonusRate = 1.15f;
            info._text = $"RGB×{info._bonusRate}";
            return info;
        }
    }
}