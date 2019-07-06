using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using URandom = UnityEngine.Random;

namespace Target
{

    public class FishController : MonoBehaviour
    {
        // 定数
        private const float kSlowTime = 2f;
        private const float kWaveWidth = 2f;
        private const float kWaveCycle = 2f;
        private const float kWaveWidthEscape = 0.5f;
        private const float kWaveCycleEscape = 0.1f;
        private const float kEscapeTime = 1f;
        private const float kEscapeSpeed = 60f;
        private const float kAwareRange = 15f;
        private const float kAwareBySpeedRate = 5f;
        private const float kSpeedBaseMin = 10f;
        private const float kSpeedBaseMax = 50f;
        private const float kArriveSlowSpeedMin = 0.2f;

        // メンバ変数　非実行時にエディタでいじる気ないけど値が見たかったりするやつは public に。
        [SerializeField]
        private Transform _imageTransform;
        private Vector2 _goalPos;
        private Vector2 _escapeVelocity;
        private float _waveTime;
        public float _waveWidth;
        public float _waveCycle;
        public float _moveSpeedBasic; // 1秒で進む距離
        public float _awareRange;
        private Timer _slowTimer;
        private Timer _escapeTimer;

        private enum State
        {
            MoveAround,
            Escape
        }
        State _state;

        static Player.PlayerController Player = null;

        // Start is called before the first frame update
        void Start()
        {
            ChooseGoalPos();
            _awareRange = kAwareRange;
            _moveSpeedBasic = URandom.Range(kSpeedBaseMin, kSpeedBaseMax);
            _slowTimer = new Timer();
            _escapeTimer = new Timer();
            ToMoveAround();
            if (Player == null)
            {
                Player = GameObject.FindGameObjectWithTag("Player")?.GetComponent<Player.PlayerController>();
            }
        }

        // Update is called once per frame
        void Update()
        {
            switch (_state)
            {
                case State.MoveAround:
                    UpdateMoveAround();
                    break;
                case State.Escape:
                    UpdateEscape();
                    break;
                default: Debug.Assert(false, "Invalid State " + (int)_state); break;
            }

            CheckToEscape();
        }

        private void ToMoveAround()
        {
            _waveTime = 0f;
            _waveWidth = kWaveWidth;
            _waveCycle = kWaveCycle;
            _slowTimer.Start(kSlowTime);
            _state = State.MoveAround;
        }

        private void ToEscape(Vector2 diffFromPlayer)
        {
            _waveTime = 0f;
            _waveWidth = kWaveWidthEscape;
            _waveCycle = kWaveCycleEscape;
            _escapeVelocity = diffFromPlayer.normalized * kEscapeSpeed;
            _escapeTimer.Start(kEscapeTime);
            _state = State.Escape;
        }

        private void UpdateMoveAround()
        {
            MoveToGoal();
            WavingMove();
            _slowTimer.Update();
        }

        private void UpdateEscape()
        {
            WavingMove();

            Vector2 pos = this.transform.position;
            pos += _escapeVelocity * Time.deltaTime;

            _escapeTimer.Update();
            if (_escapeTimer.IsEnd)
            {
                ToMoveAround();
            }

            SetPosAndDirection(pos, _escapeVelocity);
        }

        // プレイヤーが来たら逃げる！プレイヤーの移動速度が速いと遠くでも逃げる
        private void CheckToEscape()
        {
            Vector2 playerPos = (Player == null) ? Vector2.zero : Player.Position;
            Vector2 myPos = transform.position;
            Vector2 diffFromPlayer = myPos - playerPos;

            if (diffFromPlayer.magnitude <= _awareRange + Player.MovedVelocity.magnitude * kAwareBySpeedRate)
            {
                ToEscape(diffFromPlayer);
            }
        }

        // 上下揺れ
        private void WavingMove()
        {
            _waveTime = Mathf.Repeat(_waveTime + Time.deltaTime, _waveCycle);

            // 単純な上下運動
            _imageTransform.localPosition = Vector2.up * Mathf.Sin(Mathf.PI * 2f * _waveTime / _waveCycle) * _waveWidth;

        }

        // 周遊時の次の移動目的地を決める
        private void ChooseGoalPos()
        {
            Util.Bounds screenBounds = Util.GetScreenBounds();
            _goalPos.x = URandom.Range(1f, screenBounds._right) * -Mathf.Sign(_goalPos.x); // 最低でも画面右側と左側を行ったり来たりするように
            _goalPos.y = URandom.Range(screenBounds._bottom, screenBounds._top);
        }

        // 次の移動目的地まで移動し、着いたら次を決める
        private void MoveToGoal()
        {
            Vector2 pos = this.transform.position;
            Vector2 diff = (_goalPos - pos);
            float moveSpeed = _moveSpeedBasic * Time.deltaTime;
            float diffLength = diff.magnitude;

            if (moveSpeed < diffLength)
            {
                if (diffLength < _moveSpeedBasic)
                { // 近づいたらゆっくりに
                    pos += diff / diffLength * moveSpeed * Math.Max(diffLength / _moveSpeedBasic, kArriveSlowSpeedMin);
                }
                else
                {  // 通常
                    pos += diff / diffLength * moveSpeed * _slowTimer.NormalizedTime;
                }
            }
            else
            { // 到着
                pos = _goalPos;
                _slowTimer.Start(kSlowTime);
                ChooseGoalPos();
            }

            SetPosAndDirection(pos, diff);
        }

        // 位置と向き合わせ
        private void SetPosAndDirection(Vector2 pos, Vector2 velocity)
        {
            transform.position = pos;
            if (0.0f < velocity.x) { transform.localScale = new Vector3(-1f, 1f, 1f); }
            else if (velocity.x < 0.0f) { transform.localScale = new Vector3(1f, 1f, 1f); }
        }
    }

}
