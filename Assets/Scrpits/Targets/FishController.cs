using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using URandom = UnityEngine.Random;

public class FishController : MonoBehaviour
{
    private const float kSlowTime = 2f;
    private const float kWaveWidth = 2f;
    [SerializeField]
    private Transform _imageTransform;

    private Vector2 _targetPos;
    private float _waveTime;
    public float _waveCycle;
    public float _moveSpeedBasic; // 1秒で進む距離
    private Timer _slowTimer;
    

    // Start is called before the first frame update
    void Start()
    {
        ChooseTargetPos();
        _moveSpeedBasic = URandom.Range(10f, 50f);
        _waveCycle = 2.0f;
        _slowTimer = new Timer();
    }

    // Update is called once per frame
    void Update()
    {
        MoveToTarget();
        WavingMove();
        _slowTimer.Update();
    }

    private void WavingMove()
    {
        _waveTime = Mathf.Repeat(_waveTime + Time.deltaTime, _waveCycle);

        // 進行方向に対して垂直に動かすならこっち
        // Vector2 fishPos = this.transform.position;
        // Vector2 diff = _targetPos - fishPos;
        // var newPos = Vector2.Perpendicular(fishPos).normalized * Mathf.Sin(Mathf.PI * 2f * _waveTime / _waveCycle) * kWaveWidth;
        // Vector2 localPos = _imageTransform.localPosition;
        // _imageTransform.localPosition = Vector2.Lerp(localPos, newPos, _slowTimer.NormalizedTime);

        // 単純な上下運動
        _imageTransform.localPosition = Vector2.up * Mathf.Sin(Mathf.PI * 2f * _waveTime / _waveCycle) * kWaveWidth;

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

        transform.position = pos;
        // 向き合わせ
        if (0.0f < diff.x) { transform.localScale = new Vector3(-1f, 1f, 1f); }
        else if (diff.x < 0.0f) { transform.localScale = new Vector3(1f, 1f, 1f); }
    }

    private void ChooseTargetPos()
    {
        Util.Bounds screenBounds = Util.GetScreenBounds();
        _targetPos.x = URandom.Range(1f, screenBounds._right) * -Mathf.Sign(_targetPos.x); // 最低でも画面右側と左側を行ったり来たりするように
        _targetPos.y = URandom.Range(screenBounds._bottom, screenBounds._top);
    }
}
