using System;
using System.Collections;
using System.Collections.Generic;
using Target;
using UnityEngine;

namespace Result
{
    public class ScoreManager : ExplicitSingleton<ScoreManager>
    {
        private const int kEachColorScore = 50;
        private readonly int[] kBaseScore = {
          7,  // zako1
          5,  // zako2
          6,  // zako3,
          10, // katsuo
          20, // maguro
          25, // mekaziki
          10, // sake
        };

        private int[][] _fishList;
        private bool[] _colorList;

        // コンストラクタでシングルトン登録。最初のシーンロード前に一度だけ呼ばれる。
        public ScoreManager()
        {
            _fishList = new int[(Enum.GetValues(typeof(FishType)).Length)][];
            for (int idx = 0; idx < _fishList.Length; idx++)
            {
                _fishList[idx] = new int[Enum.GetValues(typeof(SizeType)).Length];
            }
            _colorList = new bool[Enum.GetValues(typeof(ColorType)).Length];
            ExplicitSingleton<ScoreManager>.Instance = this;

            SetDebugData();
        }

        [System.Diagnostics.Conditional("UNITY_EDITOR")]
        // 直接リザルトからスタートした時はResetされないのを利用
        private void SetDebugData()
        {
            for (int fishType = 0; fishType < _fishList.Length; fishType++)
            {
                for (int sizeType = 0; sizeType < _fishList[0].Length; sizeType++)
                {
                    _fishList[fishType][sizeType] = kDebugFishList[fishType, sizeType];
                }
            }
            for (int i = 0; i < _colorList.Length; i++)
            {
                _colorList[i] = kDebugColor[i];
            }
        }


        // 前回の結果を消去
        public void Reset()
        {
            for (int fishType = 0; fishType < _fishList.Length; fishType++)
            {
                for (int sizeType = 0; sizeType < _fishList[0].Length; sizeType++)
                {
                    _fishList[fishType][sizeType] = 0;
                }
            }
            for (int i = 0; i < _colorList.Length; i++)
            {
                _colorList[i] = false;
            }
        }

        // 捕まえた魚の登録
        // @todo 魚の種と色をセットで覚えておくところも作らないとストレートフラッシュが判定できない・・・
        public void RegisterCaughtFish(FishProperty property)
        {
            _colorList[(int)property._colorType] = true;
            _fishList[(int)property._fishType][(int)property._sizeType] += 1;
        }

        // 捕獲した魚情報からあれこれ計算してスコアを計算する処理
        public ScoreData CalculateScore()
        {
            var score = new ScoreData();
            // 魚の数を反映
            Debug.Assert(score._fishSNumList.Length == _fishList.Length, $"{score._fishSNumList.Length} == ${_fishList.Length}");
            for (int i = 0; i < score._fishSNumList.Length; i++) { score._fishSNumList[i] = _fishList[i][0]; }
            Debug.Assert(score._fishMNumList.Length == _fishList.Length, $"{score._fishMNumList.Length} == ${_fishList.Length}");
            for (int i = 0; i < score._fishMNumList.Length; i++) { score._fishMNumList[i] = _fishList[i][1]; }
            Debug.Assert(score._fishLNumList.Length == _fishList.Length, $"{score._fishLNumList.Length} == ${_fishList.Length}");
            for (int i = 0; i < score._fishLNumList.Length; i++) { score._fishLNumList[i] = _fishList[i][2]; }

            // 魚種類ごとの合計スコアを計算して反映
            for (int fishType = 0; fishType < _fishList.Length; fishType++)
            {
                int scoreSum = 0;
                for (int sizeType = 0; sizeType < _fishList[0].Length; sizeType++)
                {
                    scoreSum += kBaseScore[fishType] * (sizeType + 1) * _fishList[fishType][sizeType];
                }
                score._fishScoreSumList[fishType] = scoreSum;
            }
            // 色の獲得状況を反映
            for (int i = 0; i < score._colorGetList.Length; i++) { score._colorGetList[i] = _colorList[i]; }
            // 色の獲得状況に応じたスコアを計算して反映
            foreach (var isGet in score._colorGetList)
            {
                if (isGet) { score._colorScore += kEachColorScore; }
            }

            //-----------
            // 役判定
            CalculateColorSpecial(score);
            CalculateSizeSpecial(score);
            //-----------

            // 基本スコアを合計
            foreach (var fishScore in score._fishScoreSumList)
            {
                score._total += fishScore;
            }
            score._total += score._colorScore;

            // 役によるボーナスを反映
            float scoreTotalFloat = score._total;
            scoreTotalFloat *= score._colorSpecial._bonusRate;
            scoreTotalFloat *= score._sizeSpecial._bonusRate;
            scoreTotalFloat *= score._maguroSpecial._bonusRate;
            scoreTotalFloat *= score._toroSpecial._bonusRate;
            scoreTotalFloat *= score._munielSpecial._bonusRate;
            scoreTotalFloat *= score._tatakiSpecial._bonusRate;
            scoreTotalFloat *= score._specialSpecial._bonusRate;
            score._total = Mathf.RoundToInt(scoreTotalFloat);

            return score;
        }

        // 色に関する役は一番効果が高いやつひとつだけ採用
        private void CalculateColorSpecial(ScoreData score)
        {
            // レインボーALL判定
            if (_colorList[(int)ColorType.Blue]
             && _colorList[(int)ColorType.Yellow]
             && _colorList[(int)ColorType.Red]
             && _colorList[(int)ColorType.Green]
             && _colorList[(int)ColorType.Orange]
             && _colorList[(int)ColorType.Indigo]
             && _colorList[(int)ColorType.Violet]
             && !_colorList[(int)ColorType.White]
             && !_colorList[(int)ColorType.Gray]
             && !_colorList[(int)ColorType.Black])
            {
                score._colorSpecial = SpecialInfo.RainbowAll();
                return;
            }

            // モノクロALL判定
            if (!_colorList[(int)ColorType.Blue]
             && !_colorList[(int)ColorType.Yellow]
             && !_colorList[(int)ColorType.Red]
             && !_colorList[(int)ColorType.Green]
             && !_colorList[(int)ColorType.Orange]
             && !_colorList[(int)ColorType.Indigo]
             && !_colorList[(int)ColorType.Violet]
             && _colorList[(int)ColorType.White]
             && _colorList[(int)ColorType.Gray]
             && _colorList[(int)ColorType.Black])
            {
                score._colorSpecial = SpecialInfo.MonochromeAll();
                return;
            }

            // レインボー判定
            if (_colorList[(int)ColorType.Blue]
             && _colorList[(int)ColorType.Yellow]
             && _colorList[(int)ColorType.Red]
             && _colorList[(int)ColorType.Green]
             && _colorList[(int)ColorType.Orange]
             && _colorList[(int)ColorType.Indigo]
             && _colorList[(int)ColorType.Violet])
            {
                score._colorSpecial = SpecialInfo.Rainbow();
                return;
            }

            // モノクロ判定
            if (_colorList[(int)ColorType.White]
             && _colorList[(int)ColorType.Gray]
             && _colorList[(int)ColorType.Black])
            {
                score._colorSpecial = SpecialInfo.Monochrome();
                return;
            }

            // RGB判定
            if (_colorList[(int)ColorType.Blue]
             && _colorList[(int)ColorType.Red]
             && _colorList[(int)ColorType.Green])
            {
                score._colorSpecial = SpecialInfo.RGB();
                return;
            }

            // 信号機判定
            if ((_colorList[(int)ColorType.Blue]
             || _colorList[(int)ColorType.Green])
             && _colorList[(int)ColorType.Yellow]
             && _colorList[(int)ColorType.Red])
            {
                score._colorSpecial = SpecialInfo.TrafficLight();
                return;
            }
        }

         // サイズ揃えは揃った分だけ効果アップ
        private void CalculateSizeSpecial(ScoreData score)
        {
            int straightNum = 0;
            for (int type = 0; type < _fishList.Length; type++)
            {
                bool isStraight = true;
                for (int size = 0; size < _fishList[0].Length; size++)
                {
                    if (_fishList[type][size] <= 0)
                    {
                        isStraight = false;
                        break;
                    }
                }
                if (isStraight) { ++straightNum; }
            }
            Debug.Assert(0 <= straightNum && straightNum <= Enum.GetValues(typeof(FishType)).Length);
            if (0 < straightNum)
            {
                score._sizeSpecial = SpecialInfo.Straight(straightNum);
            }
        }


        readonly int[,] kDebugFishList =
        {
            { 1, 1, 1 }, // zako1
            { 1, 1, 1 }, // zako2
            { 1, 1, 1 }, // zako3,
            { 1, 1, 1 }, // katsuo
            { 1, 1, 1 }, // maguro
            { 1, 1, 1 }, // mekaziki
            { 1, 1, 1 }, // sake
        };
        readonly bool[] kDebugColor =
        {
            true, // Blue
            true, // Yellow
            true, // Red
            true, // Green
            true, // Orange
            true, // Indigo
            true, // Violet
            true, // White
            true, // Gray
            true, // Black
        };
    }
}