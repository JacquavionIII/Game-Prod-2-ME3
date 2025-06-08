using UnityEngine;
using UnityEngine.SceneManagement;


//the manager just for the playtest tutorial stuff
public class TutorialColliderManager : MonoBehaviour
{
    //variables:
    [SerializeField] private PlayerController playerController;
    [SerializeField] private GameObject playerCanvas;

    [SerializeField] private GameObject attackCanvasText;
    [SerializeField] private GameObject dashCanvasText;
    [SerializeField] private GameObject healCanvasText;

    //this will be to close the canvas text when the attack text is shown
    public void TutorialColliderOneButton()
    {
        attackCanvasText.SetActive(false);
        //to set the game back to resumed
        Time.timeScale = 1f;
        playerCanvas.SetActive(true);
        playerController.enabled = true;
    }

    //this will be to close the canvas text when the dash text is shown
    public void TutorialColliderTwoButton()
    {
        dashCanvasText.SetActive(false);
        //to set the game back to resumed
        Time.timeScale = 1f;
        playerCanvas.SetActive(true);
        playerController.enabled = true;
    }

    //this will be to close the canvas text when the heal text is shown
    public void TutorialColliderThreeButton() 
    {
        healCanvasText.SetActive(false);
        //to set the game back to resumed
        Time.timeScale = 1f;
        playerCanvas.SetActive(true);
        playerController.enabled = true;
    }

    public void RestartPlaytestButton()
    {
        SceneManager.LoadScene("Game Scene");
    }

    public void MainMenuButton()
    {
        SceneManager.LoadScene("Main Menu");
    }
}
