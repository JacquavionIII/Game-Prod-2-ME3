using UnityEngine;

public class TutorialColliderScript : MonoBehaviour
{
    [SerializeField] private GameObject tutorialText;
    [SerializeField] private GameObject playerCanvas;
    [SerializeField] private PlayerController playerController;
    [SerializeField] private GameObject tutorialCollider;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            playerController.enabled = false;
            tutorialText.SetActive(true);
            tutorialCollider.SetActive(false);
            playerCanvas.SetActive(false);
            Time.timeScale = 0f;
        }
    }
}
