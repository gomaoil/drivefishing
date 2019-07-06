using System;
using UnityEngine;

    public static class Util

{
    public struct Bounds
    {
        public float _left;
        public float _right;
        public float _bottom;
        public float _top;
    }

    static public Vector2 ClampInScreen(Vector2 pos)
    {
        // マウス位置をスクリーン座標からワールド座標に変換する
        var targetPos = Camera.main.ScreenToWorldPoint(pos);
        var screenWidth = Camera.main.orthographicSize * Camera.main.aspect;
        var screenHeight = Camera.main.orthographicSize;

        // X, Y座標の範囲を制限する
        targetPos.x = Mathf.Clamp(targetPos.x, -screenWidth, screenWidth);
        targetPos.y = Mathf.Clamp(targetPos.y, -screenHeight, screenHeight);

        // Z座標を修正する
        targetPos.z = 0f;

        return targetPos;
    }

    static public Bounds GetScreenBounds()
    {
        // マウス位置をスクリーン座標からワールド座標に変換する
        var screenWidth = Camera.main.orthographicSize * Camera.main.aspect;
        var screenHeight = Camera.main.orthographicSize;
        return new Bounds() { _left = -screenWidth, _right = screenWidth, _bottom = -screenHeight, _top = screenHeight };
    }
}
