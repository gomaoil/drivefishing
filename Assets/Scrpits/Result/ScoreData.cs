using System;
using Target;

namespace Result
{
    public class ScoreData
    {
        public int[] _fishSNumList;
        public int[] _fishMNumList;
        public int[] _fishLNumList;
        public int[] _fishScoreSumList;
        public bool[] _colorGetList;
        public int _colorScore;
        public SpecialInfo _colorSpecial;
        public SpecialInfo _sizeSpecial;
        public SpecialInfo _maguroSpecial;
        public SpecialInfo _toroSpecial;
        public SpecialInfo _munielSpecial;
        public SpecialInfo _tatakiSpecial;
        public SpecialInfo _tunaSpecial;
        public SpecialInfo _specialSpecial;
        public int _total;

        public ScoreData()
        {
            _fishSNumList = new int[Enum.GetValues(typeof(FishType)).Length];
            _fishMNumList = new int[Enum.GetValues(typeof(FishType)).Length];
            _fishLNumList = new int[Enum.GetValues(typeof(FishType)).Length];
            _fishScoreSumList = new int[Enum.GetValues(typeof(FishType)).Length];
            _colorGetList = new bool[Enum.GetValues(typeof(ColorType)).Length];
            _colorSpecial = new SpecialInfo();
            _sizeSpecial = new SpecialInfo();
            _maguroSpecial = new SpecialInfo();
            _toroSpecial = new SpecialInfo();
            _munielSpecial = new SpecialInfo();
            _tatakiSpecial = new SpecialInfo();
            _tunaSpecial = new SpecialInfo();
            _specialSpecial = new SpecialInfo();
        }
    }
}