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

        private enum FishType
        {
            katsuo,
            maguro,
            mekaziki,
            sake,
            zako1,
            zako2,
            zako,
        };

        void Start()
        {
            var screenBounds = Util.GetScreenBounds();
            for (int i = 0; i < _initialFishNum; i++)
            {
                GameObject fish = Instantiate(_prefabFish,
                    new Vector3(URandom.Range(screenBounds._left, screenBounds._right), URandom.Range(screenBounds._bottom, screenBounds._top), 0f),
                    Quaternion.identity);
                var fishType = (FishType)URandom.Range(0, Enum.GetValues(typeof(FishType)).Length);
                string fishName = fishType.ToString();
                fish.GetComponentInChildren<SpriteRenderer>().sprite = Resources.Load<Sprite>(fishName);
                fish.name = fishName;
            }
        }

        void Update()
        {
        }
    }

}
