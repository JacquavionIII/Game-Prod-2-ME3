using UnityEngine;

public class EndPlaytestScript : MonoBehaviour
{
    [SerializeField] private PlayerController playerController;
    [SerializeField] private GameObject playerCanvas;
    [SerializeField] private GameObject endPlaytestText;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            playerController.enabled = false;
            playerCanvas.SetActive(false);
            endPlaytestText.SetActive(true);
        }
    }
}
