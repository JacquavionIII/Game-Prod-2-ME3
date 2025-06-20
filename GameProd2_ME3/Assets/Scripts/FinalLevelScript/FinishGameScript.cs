using UnityEngine;

public class FinishGameScript : MonoBehaviour
{
    [SerializeField] private GameObject crawler;
    [SerializeField] private GameObject bat;
    [SerializeField] private GameObject charger;

    [SerializeField] private GameObject finishGameCanvas;
    [SerializeField] private PlayerController playerController;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (crawler == null &&
            bat == null &&
            charger == null)
        {
            finishGameCanvas.SetActive(true);
            playerController.enabled = false;
        }
    }
}
