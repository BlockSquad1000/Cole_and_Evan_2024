using UnityEngine;

public abstract class ScriptableObjectSingleton<T> : ScriptableObject where T : ScriptableObject
{
    public static T instance = null;

    public static T Instance
    {
        get
        {
            if(instance == null)
            {
                T[] results = Resources.FindObjectsOfTypeAll<T>();
                if(results.Length == 0)
                {
                    Debug.LogError("SingletonScriptableObject Instance results length is 0 for type " + typeof(T).ToString() + ".");
                }
                if(results.Length > 1)
                {
                    Debug.LogError("SingletonScriptableObject Instance results length is greater than 1 for type " + typeof(T).ToString() + ".");
                }

                instance = results[0];
            }
            return instance;
        }
    }
}
