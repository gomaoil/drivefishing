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
          7, // zako1
          5, // zako2
          6, // zako3,           
          10, // katsuo
          20, // maguro
          25, // mekaziki
          10, // sake
        };

        private int[][] _fishList;
        private bool[] _colorList;

        public ScoreManager()
        {
            _fishList = new int[(Enum.GetValues(typeof(FishType)).Length)][];
            for (int idx = 0; idx < _fishList.Length; idx++)
            {
                _fishList[idx] = new int[Enum.GetValues(typeof(SizeType)).Length];
            }
            _colorList = new bool[Enum.GetValues(typeof(ColorType)).Length];
            ExplicitSingleton<ScoreManager>.Instance = this;
        }

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

        public void RegisterCaughtFish(FishProperty property)
        {
            _colorList[(int)property._colorType] = true;
            _fishList[(int)property._fishType][(int)property._sizeType] += 1;
        }

        // 捕獲した魚情報からあれこれ計算してスコアを計算する処理
        public ScoreData CalculateScore()
        {
            var score = new ScoreData();

            Debug.Assert(score._fishSNumList.Length == _fishList.Length, $"{score._fishSNumList.Length} == ${_fishList.Length}");
            Debug.Assert(score._fishMNumList.Length == _fishList.Length, $"{score._fishMNumList.Length} == ${_fishList.Length}");
            Debug.Assert(score._fishLNumList.Length == _fishList.Length, $"{score._fishLNumList.Length} == ${_fishList.Length}");
            for (int i = 0; i < score._fishSNumList.Length; i++) { score._fishSNumList[i] = _fishList[i][0]; }
            for (int i = 0; i < score._fishMNumList.Length; i++) { score._fishMNumList[i] = _fishList[i][1]; }
            for (int i = 0; i < score._fishLNumList.Length; i++) { score._fishLNumList[i] = _fishList[i][2]; }
            for (int fishType = 0; fishType < _fishList.Length; fishType++)
            {
                int scoreSum = 0;
                for (int sizeType = 0; sizeType < _fishList[0].Length; sizeType++)
                {
                    scoreSum += kBaseScore[fishType] * (sizeType + 1) * _fishList[fishType][sizeType];
                }
                score._fishScoreSumList[fishType] = scoreSum;
            }
            for (int i = 0; i < score._colorGetList.Length; i++) { score._colorGetList[i] = _colorList[i]; }
            foreach (var isGet in score._colorGetList)
            {
                if (isGet) { score._colorScore += kEachColorScore; }
            }

            //-----------
            // 役判定が入る予定
            //-----------

            foreach (var fishScore in score._fishScoreSumList)
            {
                score._total += fishScore;
            }
            score._total += score._colorScore;

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
    }
}