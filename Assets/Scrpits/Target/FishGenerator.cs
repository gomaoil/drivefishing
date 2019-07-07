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

                FishProperty property = fish.GetComponent<FishProperty>();
                property._fishType = (FishType)URandom.Range(0, Enum.GetValues(typeof(FishType)).Length);
                property._colorType = (ColorType)URandom.Range(0, Enum.GetValues(typeof(ColorType)).Length);
                property._sizeType = (SizeType)URandom.Range(0, Enum.GetValues(typeof(SizeType)).Length);

                string fishName = property._fishType.ToString();
                SpriteRenderer spriteRenderer = fish.GetComponentInChildren<SpriteRenderer>();
                spriteRenderer.sprite = Resources.Load<Sprite>(fishName);
                spriteRenderer.material.SetColor("_ComplementColor0", property._colorType.GetColor());
                if (property._colorType == ColorType.White) { spriteRenderer.material.SetColor("_ComplementColor1", Color.gray); }
                fish.transform.localScale = Vector3.one * property._sizeType.GetScale();
            }
        }
    }

}
