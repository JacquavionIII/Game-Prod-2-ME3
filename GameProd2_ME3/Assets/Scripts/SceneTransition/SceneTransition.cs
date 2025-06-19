using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneTransition : MonoBehaviour
{
    [SerializeField] private string transitionTo;

    [SerializeField] private Transform startPoint;

    [SerializeField] private Vector2 exitDirection;

    [SerializeField] float exitTime;

    private void Start()
    {
        if (transitionTo == GameManager.instance.transitionedFromScene)
        {
            PlayerController.instance.transform.position = startPoint.position;

            StartCoroutine(PlayerController.instance.WalkIntoNewScene(exitDirection, exitTime));
        }
        StartCoroutine(FadeManager.instance.sceneFader.Fade(Scenefader.FadeDirection.Out));
    }

    private void OnTriggerEnter2D(Collider2D _other)
    {
        if (_other.CompareTag("Player"))
        {
            GameManager.instance.transitionedFromScene = SceneManager.GetActiveScene().name;

            PlayerController.instance.pState.cutscene = true;
            PlayerController.instance.pState.invincibleFrames = true;

            StartCoroutine(FadeManager.instance.sceneFader.FadeAndLoadScene(Scenefader.FadeDirection.In, transitionTo));
        }
    }
}
