using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Result
{

    public class ResultSceneManager : MonoBehaviour
    {
        [SerializeField]
        private TMPro.TextMeshProUGUI _totalScore;
        [SerializeField]
        private List<TMPro.TextMeshProUGUI> _fishScoreS;
        [SerializeField]
        private List<TMPro.TextMeshProUGUI> _fishScoreM;
        [SerializeField]
        private List<TMPro.TextMeshProUGUI> _fishScoreL;
        [SerializeField]
        private List<TMPro.TextMeshProUGUI> _fishScoreSum;
        [SerializeField]
        private List<GameObject> _colorPanel;
        [SerializeField]
        private TMPro.TextMeshProUGUI _colorScore;
        [SerializeField]
        private TMPro.TextMeshProUGUI _colorSpecial;
        [SerializeField]
        private TMPro.TextMeshProUGUI _sizeSpecial;
        [SerializeField]
        private TMPro.TextMeshProUGUI _maguroSpecial;
        [SerializeField]
        private TMPro.TextMeshProUGUI _toroSpecial;
        [SerializeField]
        private TMPro.TextMeshProUGUI _munielSpecial;
        [SerializeField]
        private TMPro.TextMeshProUGUI _tatakiSpecial;
        [SerializeField]
        private TMPro.TextMeshProUGUI _tunaSpecial;
        [SerializeField]
        private TMPro.TextMeshProUGUI _specialSpecial;
        private Timer _waitTimer;

        // Start is called before the first frame update
        void Start()
        {
            var scoreData = ScoreManager.Instance.CalculateScore();
            _waitTimer = new Timer();

            SetFishScores(scoreData);
            SetColorScore(scoreData);
            SetColorSpecial(scoreData);
            SetSizeSpecial(scoreData);
            SetMaguroSpecial(scoreData);
            SetToroSpecial(scoreData);
            SetMunielSpecial(scoreData);
            SetTatakiSpecial(scoreData);
            SetTunaSpecial(scoreData);
            SetSpecialSpecial(scoreData);
            
            _waitTimer.Start(1.5f);
            _totalScore.SetText(scoreData._total.ToString());
        }

        private void SetFishScores(ScoreData scoreData)
        {
            for (int i = 0; i < _fishScoreS.Count; i++)
            {
                Debug.Assert(_fishScoreS.Count == scoreData._fishSNumList.Length);
                _fishScoreS[i].SetText(scoreData._fishSNumList[i].ToString());
            }
            for (int i = 0; i < _fishScoreM.Count; i++)
            {
                Debug.Assert(_fishScoreM.Count == scoreData._fishMNumList.Length);
                _fishScoreM[i].SetText(scoreData._fishMNumList[i].ToString());
            }
            for (int i = 0; i < _fishScoreL.Count; i++)
            {
                Debug.Assert(_fishScoreL.Count == scoreData._fishLNumList.Length);
                _fishScoreL[i].SetText(scoreData._fishLNumList[i].ToString());
            }
            for (int i = 0; i < _fishScoreSum.Count; i++)
            {
                Debug.Assert(_fishScoreSum.Count == scoreData._fishScoreSumList.Length);
                _fishScoreSum[i].SetText(scoreData._fishScoreSumList[i].ToString());
            }
        }

        private void SetColorScore(ScoreData scoreData)
        {
            for (int i = 0; i < _colorPanel.Count; i++)
            {
                Debug.Assert(_colorPanel.Count == scoreData._colorGetList.Length);
                _colorPanel[i].SetActive(scoreData._colorGetList[i]);
            }
            _colorScore.SetText(scoreData._colorScore.ToString());
        }

        private void SetColorSpecial(ScoreData scoreData)
        {
            _colorSpecial.SetText(scoreData._colorSpecial._text);
        }

        private void SetSizeSpecial(ScoreData scoreData)
        {
            _sizeSpecial.SetText(scoreData._sizeSpecial._text);
        }

        private void SetMaguroSpecial(ScoreData scoreData)
        {
            _maguroSpecial.SetText(scoreData._maguroSpecial._text);
        }

        private void SetToroSpecial(ScoreData scoreData)
        {
            _toroSpecial.SetText(scoreData._toroSpecial._text);
        }

        private void SetMunielSpecial(ScoreData scoreData)
        {
            _munielSpecial.SetText(scoreData._munielSpecial._text);
        }

        private void SetTatakiSpecial(ScoreData scoreData)
        {
            _tatakiSpecial.SetText(scoreData._tatakiSpecial._text);
        }

        private void SetTunaSpecial(ScoreData scoreData)
        {
            _tunaSpecial.SetText(scoreData._tunaSpecial._text);
        }

        private void SetSpecialSpecial(ScoreData scoreData)
        {
            _specialSpecial.SetText(scoreData._specialSpecial._text);
        }

        // Update is called once per frame
        void Update()
        {
            if (_waitTimer.IsEnd)
            {
                if (Input.GetMouseButtonUp(0))
                {
                    SceneManager.LoadScene("MainGame");
                }
            }
        }
    }

}
