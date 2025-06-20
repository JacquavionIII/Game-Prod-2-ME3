using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class FadeManager : MonoBehaviour
{
    public static FadeManager instance;

    [SerializeField] private GameObject fadeCanvas;

    public Scenefader sceneFader;

    [SerializeField] GameObject deathScreen;

    private void Awake()
    {
        deathScreen = GameObject.FindGameObjectWithTag("DeathCanvas");
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

    public IEnumerator ActivateDeathScreen()
    {
        yield return new WaitForSeconds(0.8f);
        StartCoroutine(sceneFader.Fade(Scenefader.FadeDirection.In));

        yield return new WaitForSeconds(0.8f);
        fadeCanvas.SetActive(false);
    }

    //UI shenanigans
    public IEnumerator DeactivateDeathScreen()
    {
        yield return new WaitForSeconds(0.5f);
        fadeCanvas.SetActive(true);
        StartCoroutine(sceneFader.Fade(Scenefader.FadeDirection.Out));
    }

    public void GoToSecondLevel()
    {
        SceneManager.LoadScene("SecondLevel");
    }
}
