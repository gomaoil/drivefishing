using System;
using UnityEngine;

public class Timer
{
    private float _targetTime;
    private float _startTime;
    private bool _isOnEnd;
    private bool _isEnd;

    public bool IsEnd { get => _isEnd; }
    public bool IsOnEnd { get => _isOnEnd; }
    public float PassedTime { get => Mathf.Clamp(Time.time - _startTime, 0f, _targetTime); }
    public float RestTime { get => Mathf.Clamp(_startTime + _targetTime - Time.time, 0f, _targetTime); }
    public float NormalizedTime { get => _targetTime == 0f ? 1.0f : Mathf.Clamp01((Time.time - _startTime) / _targetTime); }

    public Timer()
    {
        _startTime = Time.time;
    }

    public void Start(float time)
    {
        _startTime = Time.time;;
        _targetTime = time;
        _isEnd = false;
        _isOnEnd = false;
    }

    public void Update()
    {
        _isOnEnd = false;

        if (_targetTime <= Time.time - _startTime)
        {
            if (!_isEnd)
            {
                _isEnd = true;
                _isOnEnd = true;
            }
        }
    }
}
