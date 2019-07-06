using System.Collections;
using System.Collections.Generic;
using Target;
using UnityEngine;

namespace Result
{
    public class ScoreManager : ExplicitSingleton<ScoreManager>
    {
        private readonly int[] kBaseScore = {
          30, // katsuo
          70, // maguro
          80, // mekaziki
          30, // sake
          10, // zako1
          15, // zako2
          20  // zako3,           
        };

        private List<FishProperty> _catchList;

        public ScoreManager()
        {
            _catchList = new List<FishProperty>();
            ExplicitSingleton<ScoreManager>.Instance = this;
        }

        public void RegisterCaughtFish(FishProperty fishProperty)
        {
            _catchList.Add(fishProperty);
        }

        // 捕獲した魚情報からあれこれ計算してスコアを計算する処理
        public int CalculateScore()
        {
            int scoreTotal = 0;

            foreach (var property in _catchList)
            {
                scoreTotal += kBaseScore[(int)property._fishType];
            }

            return scoreTotal;
        }
    }
}