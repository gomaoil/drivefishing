using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using URandom = UnityEngine.Random;

public class FishController : MonoBehaviour
{
    private const float kSlowTime = 2f;
    private const float kWaveWidth = 2f;
    private const float kWaveCycle = 2f;
    private const float kWaveWidthEscape = 0.5f;
    private const float kWaveCycleEscape = 0.1f;
    private const float kEscapeSpeed = 60.0f;
    [SerializeField]
    private Transform _imageTransform;

    private Vector2 _targetPos;
    private Vector2 _escapeVelocity;
    private float _waveTime;
    public float _waveWidth;
    public float _waveCycle;
    public float _moveSpeedBasic; // 1秒で進む距離
    public float _awareRange;
    private Timer _slowTimer;
    private Timer _escapeTimer;
   
    static Player.PlayerController Player = null;

    private enum State {
        MoveAround,
        Escape
    }
    State _state;

    // Start is called before the first frame update
    void Start()
    {
        ChooseTargetPos();
        _awareRange = 15.0f;
        _moveSpeedBasic = URandom.Range(10f, 50f);
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

    private void UpdateMoveAround()
    {
        MoveToTarget();
        WavingMove();
        _slowTimer.Update();
    }


    private void CheckToEscape()
    {
        Vector2 playerPos = (Player == null) ? Vector2.zero : Player.Position;
        Vector2 myPos = transform.position;
        Vector2 diffFromPlayer = myPos - playerPos;

        if (diffFromPlayer.magnitude <= _awareRange + Player.MovedVelocity.magnitude * 5f)
        {
            ToEscape(diffFromPlayer);
        }
    }

    private void ToEscape(Vector2 diffFromPlayer)
    {
        _waveTime = 0f;
        _waveWidth = kWaveWidthEscape;
        _waveCycle = kWaveCycleEscape;
        _escapeVelocity = diffFromPlayer.normalized * kEscapeSpeed;
        _escapeTimer.Start(1.0f);
        _state = State.Escape;
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

    private void WavingMove()
    {
        _waveTime = Mathf.Repeat(_waveTime + Time.deltaTime, _waveCycle);

        // 進行方向に対して垂直に動かすならこっち
        // Vector2 fishPos = this.transform.position;
        // Vector2 diff = _targetPos - fishPos;
        // var newPos = Vector2.Perpendicular(fishPos).normalized * Mathf.Sin(Mathf.PI * 2f * _waveTime / _waveCycle) * _waveWidth;
        // Vector2 localPos = _imageTransform.localPosition;
        // _imageTransform.localPosition = Vector2.Lerp(localPos, newPos, _slowTimer.NormalizedTime);

        // 単純な上下運動
        _imageTransform.localPosition = Vector2.up * Mathf.Sin(Mathf.PI * 2f * _waveTime / _waveCycle) * _waveWidth;

    }

    private void MoveToTarget()
    {
        Vector2 pos = this.transform.position;
        Vector2 diff = (_targetPos - pos);
        float moveSpeed = _moveSpeedBasic * Time.deltaTime;
        float diffLength = diff.magnitude;

        if (moveSpeed < diffLength)
        {
            if (diffLength < _moveSpeedBasic)
            { // 近づいたらゆっくりに
                pos += diff / diffLength * moveSpeed * Math.Max(diffLength / _moveSpeedBasic, 0.2f);
            }
            else
            {  // 通常
                pos += diff / diffLength * moveSpeed * _slowTimer.NormalizedTime;
            }
        }
        else
        { // 到着
            pos = _targetPos;
            _slowTimer.Start(kSlowTime);
            ChooseTargetPos();
        }

        SetPosAndDirection(pos, diff);
    }

    private void SetPosAndDirection(Vector2 pos, Vector2 velocity)
    {
        transform.position = pos;
        // 向き合わせ
        if (0.0f < velocity.x) { transform.localScale = new Vector3(-1f, 1f, 1f); }
        else if (velocity.x < 0.0f) { transform.localScale = new Vector3(1f, 1f, 1f); }
    }

    private void ChooseTargetPos()
    {
        Util.Bounds screenBounds = Util.GetScreenBounds();
        _targetPos.x = URandom.Range(1f, screenBounds._right) * -Mathf.Sign(_targetPos.x); // 最低でも画面右側と左側を行ったり来たりするように
        _targetPos.y = URandom.Range(screenBounds._bottom, screenBounds._top);
    }
}
