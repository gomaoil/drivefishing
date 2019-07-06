using System;
using UnityEngine;

namespace Player
{

    public class PlayerController : MonoBehaviour
    {
        // 補間の強さ（0f～1f） 。0なら追従しない。1なら遅れなしに追従する。
        [SerializeField, Range(0f, 1f)]
        private float _followStrength;

        private void Update()
        {
            var targetPos = Util.ClampInScreen(Input.mousePosition);

            // このスクリプトがアタッチされたゲームオブジェクトを、マウス位置に線形補間で追従させる
            transform.position = Vector3.Lerp(transform.position, targetPos, _followStrength);
        }
    }

}

