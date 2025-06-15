using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    public static bool GameIsPaused = false;

    [SerializeField] private GameObject playerCanvas;

    public GameObject pauseMenuUI;

    //Input system
    private InputAction m_pauseActionPlayer;
    private InputAction m_pauseActionUI;
    //getting access to the player controller script to access the input system
    private PlayerController playerController;

    private void Awake()
    {
        m_pauseActionPlayer = InputSystem.actions.FindAction("Player/Pause");
        m_pauseActionUI = InputSystem.actions.FindAction("UI/Pause");
        playerController = FindFirstObjectByType<PlayerController>();
    }

    void Update()
    {
        if (m_pauseActionPlayer.WasPressedThisFrame())
        {
            playerController.inputActions.FindActionMap("Player").Disable();
            playerController.inputActions.FindActionMap("UI").Enable();
            pauseMenuUI.SetActive(true);
            playerCanvas.SetActive(false);
            Time.timeScale = 0f; // Freeze the game
            playerController.enabled = false;
        }
        else if (m_pauseActionUI.WasPressedThisFrame())
        {
            playerController.inputActions.FindActionMap("Player").Enable();
            playerController.inputActions.FindActionMap("UI").Disable();
            pauseMenuUI.SetActive(false);
            playerCanvas.SetActive(true);
            Time.timeScale = 1f; // Resume the game
            playerController.enabled = true;
        }
    }

    public void Resume()
    {
        pauseMenuUI.SetActive(false);
        playerCanvas.SetActive(true);
        Time.timeScale = 1f; // Resume the game
        GameIsPaused = false;
        playerController.enabled = true;
    }

    void Pause()
    {
        pauseMenuUI.SetActive(true);
        Time.timeScale = 0f; // Freeze the game
        GameIsPaused = true;
        playerController.enabled = false;
    }

    public void LoadMenu()
    {
        Time.timeScale = 1f; 
        SceneManager.LoadScene("Main Menu");
    }

    public void QuitGame()
    { 
        Application.Quit();
    }
}
