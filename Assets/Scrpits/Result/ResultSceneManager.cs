using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Result
{

    public class ResultSceneManager : MonoBehaviour
    {
        [SerializeField]
        TMPro.TextMeshProUGUI _totalScore;
        Timer _waitTimer;

        // Start is called before the first frame update
        void Start()
        {
            _waitTimer = new Timer();

            _waitTimer.Start(1.5f);
            _totalScore.SetText(ScoreManager.Instance.CalculateScore().ToString());
        }

        // Update is called once per frame
        void Update()
        {
            if (Input.GetMouseButtonUp(0))
            {
                SceneManager.LoadScene("MainGame");
            }
        }
    }

}
