using System;
using UnityEngine;
using URandom = UnityEngine.Random;

namespace Target
{
    public class FishGenerator : MonoBehaviour
    {
        [SerializeField]
        GameObject _prefabFish;
        [SerializeField]
        int _initialFishNum = 20;

        void Start()
        {
            var screenBounds = Util.GetScreenBounds();
            for (int i = 0; i < _initialFishNum; i++)
            {
                GameObject fish = Instantiate(_prefabFish,
                    new Vector3(URandom.Range(screenBounds._left, screenBounds._right), URandom.Range(screenBounds._bottom, screenBounds._top), 0f),
                    Quaternion.identity);

                var fishType = (FishType)URandom.Range(0, Enum.GetValues(typeof(FishType)).Length);
                var colorType = (ColorType)URandom.Range(0, Enum.GetValues(typeof(ColorType)).Length);
                var sizeType = (SizeType)URandom.Range(0, Enum.GetValues(typeof(SizeType)).Length);

                string fishName = fishType.ToString();
                SpriteRenderer spriteRenderer = fish.GetComponentInChildren<SpriteRenderer>();
                spriteRenderer.sprite = Resources.Load<Sprite>(fishName);
                spriteRenderer.material.SetColor("_ComplementColor0", colorType.GetColor());

                fish.GetComponent<FishProperty>()._fishType = fishType;
            }
        }
    }

}
