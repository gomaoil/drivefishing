using UnityEngine;

/// 生成と登録を明示的なタイミングで行うシングルトン
public class ExplicitSingleton<T> where T : class
{
    // 実体
    private static T _instance;

    // アクセス時に生成はしないで代わりにアサート。シンプルな構造。生成登録順は利用者側で制御せいや。
    public static T Instance { 
        get
        {
            Debug.Assert(_instance != null);
            return _instance;
        }
        protected set
        {
            Debug.Assert(_instance == null);
            _instance = value;
        }
    }

    // こいつのコンストラクタは直接呼べない
    protected ExplicitSingleton() {}
}