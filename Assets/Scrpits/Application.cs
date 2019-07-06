using Result;
using UnityEngine;

/// シングルトン作ったりゲーム全体の下準備や後片付けをする処理を担うつもり
public class Application
{

    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
    private static void InitializeSingleton()
    {
        new ScoreManager();
    }
}
