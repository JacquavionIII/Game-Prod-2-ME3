using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public string transitionedFromScene;

    public static GameManager instance;

    public Vector2 platformRespawnPoint;

    public Vector2 benchRespawnPoint;
    [SerializeField] Bench bench;

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
        bench = FindFirstObjectByType<Bench>();
    }

    public void RespawnPlayer()
    {
        if (bench.benchInteracted)
        {
            benchRespawnPoint = bench.transform.position;
        }           
        else
        {
            benchRespawnPoint = platformRespawnPoint;
        }
        PlayerController.instance.transform.position = benchRespawnPoint;

        StartCoroutine(FadeManager.instance.DeactivateDeathScreen());
        PlayerController.instance.Respawned();
    }
}
