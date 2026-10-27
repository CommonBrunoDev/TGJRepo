using UnityEngine;

public abstract class Singleton<T> : MonoBehaviour where T : MonoBehaviour
{
    private static T _instance;

    public static T Instance => _instance;
    protected virtual bool DoNotDestroyOnLoad => false;

    protected virtual void Awake() 
    {
        if (_instance == null)
            _instance = this as T;
        else
            Destroy(gameObject);

        if (DoNotDestroyOnLoad)
            DontDestroyOnLoad(gameObject);
    }

    protected virtual void Destroy() 
    {
        if (_instance == this)
            _instance = null;
    }
}
