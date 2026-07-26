using UnityEngine;

public class DontDestroyOnLoad : MonoBehaviour
{
    void Awake()
    {
        DontDestroyOnLoad(gameObject);

        var dontDestroyOnLoads = FindObjectsByType<DontDestroyOnLoad>();
        if(dontDestroyOnLoads.Length > 1)
        {
            Destroy(gameObject);
        }
    }
}

