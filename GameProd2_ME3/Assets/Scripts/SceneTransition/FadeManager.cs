using UnityEngine;

public class FadeManager : MonoBehaviour
{
    public static FadeManager instance;

    public Scenefader sceneFader;

    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            instance = this;
        }
        DontDestroyOnLoad(gameObject);
    }
}
