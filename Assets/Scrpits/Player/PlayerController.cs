using System;
using UnityEngine;

namespace Player
{

    public class PlayerController : MonoBehaviour
    {
        // 補間の強さ（0f～1f） 。0なら追従しない。1なら遅れなしに追従する。
        [SerializeField, Range(0f, 1f)]
        private float _followStrength;

        public Vector2 Position { get; private set; }
        public Vector2 MovedVelocity { get; private set; }

        private void Start()
        {
            transform.position = Util.ClampInScreen(Input.mousePosition);
            Position = transform.position;
        }

        private void Update()
        {
            var targetPos = Util.ClampInScreen(Input.mousePosition);

            // このスクリプトがアタッチされたゲームオブジェクトを、マウス位置に線形補間で追従させる
            transform.position = Vector3.Lerp(transform.position, targetPos, _followStrength);

            MovedVelocity = (Vector2)transform.position - Position;
            Position = transform.position;

            // 向き合わせ
            if (0.0f < MovedVelocity.x) { transform.localScale = new Vector3(-1f, 1f, 1f); }
            else if (MovedVelocity.x < 0.0f) { transform.localScale = new Vector3(1f, 1f, 1f); }
        }
    }

}

