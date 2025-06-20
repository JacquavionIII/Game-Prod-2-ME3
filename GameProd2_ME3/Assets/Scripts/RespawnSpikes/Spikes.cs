using System.Collections;
using UnityEngine;

public class Spikes : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D _other)
    {
        if (_other.CompareTag("Player"))
        {
            StartCoroutine(RespawnPoint());
        }
    }

    IEnumerator RespawnPoint()
    {
        PlayerController.instance.pState.cutscene = true;
        PlayerController.instance.pState.invincibleFrames = true;
        PlayerController.instance.playerRB.linearVelocity = Vector2.zero;
        Time.timeScale = 0;
        StartCoroutine(FadeManager.instance.sceneFader.Fade(Scenefader.FadeDirection.In));
        PlayerController.instance.TakeDamage(1);
        yield return new WaitForSecondsRealtime(1);
        PlayerController.instance.transform.position = GameManager.instance.platformRespawnPoint;
        StartCoroutine(FadeManager.instance.sceneFader.Fade(Scenefader.FadeDirection.Out));
        yield return new WaitForSecondsRealtime(FadeManager.instance.sceneFader.fadeTime);
        PlayerController.instance.pState.cutscene = false;
        PlayerController.instance.pState.invincibleFrames = false;
        Time.timeScale = 1;
    }
}
