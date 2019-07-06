using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;

namespace Player
{

    public class Fisherman : MonoBehaviour
    {
        [SerializeField]
        private TextMeshPro _countDownText;
        [SerializeField]
        private PolygonCollider2D _collider;
        private Timer _timer;
        private bool _isCountDown;

        private readonly string[] kNumber = { "0", "1", "2", "3", "4", "5", "6", "7", "8", "9" };

        // Start is called before the first frame update
        void Start()
        {
            _timer = new Timer();
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
                        _isCountDown = false;
                        _countDownText.enabled = false;
                    }
                }
            }
            else
            {
                if (Input.GetMouseButtonDown(0))
                { // 左クリックで引き上げ開始
                    _timer.Start(3f);
                    _isCountDown = true;
                    _countDownText.enabled = true;
                    SetCountDownText();
                }
            }
            
            _timer.Update();
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
                    Destroy(fish);
                    Debug.Log("Catch! : " + fish.name);
                }
            }
        }

    }
}
