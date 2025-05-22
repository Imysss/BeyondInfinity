using UnityEngine;

public abstract class Singleton<T> : MonoBehaviour where T : MonoBehaviour
{
    private static T _instance;

    public static T Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = FindObjectOfType<T>();
                if (FindObjectsOfType<T>().Length > 1)
                {
                    Debug.Log($"[Singleton]중복 인스턴스 발견: {typeof(T)}");
                }

                if (_instance == null)
                {
                    GameObject singleton = new GameObject($"{typeof(T)}");
                    _instance = singleton.AddComponent<T>();
                    DontDestroyOnLoad(singleton);
                    Debug.Log($"[SingleTon]생성: {typeof(T)}");
                }
            }

            return _instance;
        }
    }
}
