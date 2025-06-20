using UnityEngine;

public class GameManager : MonoBehaviour
{
    public string transitionedFromScene;

    public static GameManager instance;

    public Vector2 platformRespawnPoint;

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
