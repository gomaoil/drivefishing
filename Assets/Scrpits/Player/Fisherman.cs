using System.Collections;
using UnityEngine;
using TMPro;
using Result;
using Target;
using UnityEngine.SceneManagement;

namespace Player
{

    public class Fisherman : MonoBehaviour
    {
        private const float kCountDownTime = 3f;
        private const int kCatchTryNum = 3;
        // 毎回 ToString するの気が引けるし、変わってない判定するのも面倒なので
        private readonly string[] kNumber = { "0", "1", "2", "3", "4", "5", "6", "7", "8", "9" };

        [SerializeField]
        private TextMeshPro _countDownText;
        [SerializeField]
        private PolygonCollider2D _collider;
        private Timer _timer;
        private int _restTryNum;
        private bool _isCountDown;

        // Start is called before the first frame update
        void Start()
        {
            _timer = new Timer();
            _restTryNum = kCatchTryNum;
            _countDownText.enabled = false;
        }

        // Update is called once per frame
        void Update()
        {
            if (_isCountDown)
            {
                SetCountDownText();
                if (Input.GetMouseButtonDown(1))
                { // 右クリックでキャンセル！
                    _isCountDown = false;
                    _countDownText.enabled = false;
                }
                else
                {
                    if (_timer.IsOnEnd)
                    {
                        CatchLanding();
                        JudgeToResult();
                        _isCountDown = false;
                    }
                }
            }
            else
            {
                if (Input.GetMouseButtonDown(0))
                { // 左クリックで引き上げ開始
                    _timer.Start(kCountDownTime);
                    _isCountDown = true;
                    _countDownText.enabled = true;
                    SetCountDownText();
                }
            }
            
            _timer.Update();
        }

        private void JudgeToResult()
        {
            if (--_restTryNum <= 0)
            {
                Time.timeScale = 0f;
                StartCoroutine(ToResult());
            }
            else
            {
                _countDownText.enabled = false;
            }
        }

        // 終了演出待ち＆シーン移動のコルーチン
        /// @todo このクラスの責任じゃないし、もうちょっとちゃんとした終了演出を考えたい
        private IEnumerator ToResult()
        {
            _countDownText.SetText("Game Set!"); // 仮

            yield return new WaitForSecondsRealtime(2f);

            Time.timeScale = 1f;
            SceneManager.LoadScene("Result");
        }

        private void SetCountDownText()
        {
            int number = Mathf.Clamp((int) Mathf.RoundToInt(_timer.RestTime), 0, kNumber.Length);
            _countDownText.SetText(kNumber[number]);
        }

        // 水揚げ！
        private void CatchLanding()
        {
            foreach (var fish in GameObject.FindGameObjectsWithTag("Fish"))
            {
                if (_collider.OverlapPoint(fish.transform.position))
                {
                    var property = fish.GetComponent<FishProperty>();
                    Debug.Assert(property != null);
                    ScoreManager.Instance.RegisterCaughtFish(property);

                    Destroy(fish);
                }
            }
        }

    }
}
