using UnityEngine;
using UnityEngine.SceneManagement;

public class ContinueButtonScript : MonoBehaviour
{
    [Header("Story Text")]
    [SerializeField] private GameObject lineOne;
    [SerializeField] private GameObject lineOneButton;
    [SerializeField] private GameObject lineTwo;
    [SerializeField] private GameObject lineTwoButton;
    [SerializeField] private GameObject lineThree;
    [SerializeField] private GameObject lineThreeButton;
    [SerializeField] private GameObject lineFour;
    [SerializeField] private GameObject lineFourButton;
    [SerializeField] private GameObject lineFive;
    [SerializeField] private GameObject lineFiveButton;
    [SerializeField] private GameObject storyTextCanvas;
    [Space(5)]

    //for the story text
    private bool lineOneActive = false;
    private bool lineTwoActive = false;
    private bool lineThreeActive = false;
    private bool lineFourActive = false;
    private bool lineFiveActive = false;

    [Header("Controls Text")]
    [SerializeField] private GameObject controlsTextCanvas;
    [SerializeField] private GameObject cLineOne;
    [SerializeField] private GameObject cLineOneButton;
    [SerializeField] private GameObject cLineTwo;
    [SerializeField] private GameObject cLineTwoButton;
    [SerializeField] private GameObject cLineThree;
    [SerializeField] private GameObject cLineThreeButton;
    [SerializeField] private GameObject cLineFour;
    [SerializeField] private GameObject cLineFourButton;
    [SerializeField] private GameObject cLineFive;
    [SerializeField] private GameObject cLineFiveButton;

    [Header("Controls Sprites")]
    [SerializeField] private GameObject moveControls;
    [SerializeField] private GameObject dashControls;
    [SerializeField] private GameObject interactControls;
    [SerializeField] private GameObject attackControls;
    [SerializeField] private GameObject pauseControls;
    [Space(5)]

    //for the controls text
    private bool cLineOneActive = false;
    private bool cLineTwoActive = false;
    private bool cLineThreeActive = false;
    private bool cLineFourActive = false;
    private bool cLineFiveActive = false;

    [Header("Go to Game text")]
    [SerializeField] private GameObject goToGameText;
    [SerializeField] private GameObject goToGameButton;


    //for story text
    public void LineOneActivate()
    {
        lineOneActive = true;
        if (lineOneActive)
        {
            lineOne.SetActive(true);
            lineOneButton.SetActive(false);
            lineTwoButton.SetActive(true);
        }
    }

    public void LineTwoActivate()
    {
        lineTwoActive = true;
        if (lineTwoActive)
        {
            lineTwo.SetActive(true);
            lineTwoButton.SetActive(false);
            lineThreeButton.SetActive(true);
        }
    }

    public void LineThreeActivate()
    {
        lineThreeActive = true;
        if (lineThreeActive)
        {
            lineThree.SetActive(true);
            lineThreeButton.SetActive(false);
            lineFourButton.SetActive(true);
        }
    }

    public void LineFourActivate()
    {
        lineFourActive = true;
        if (lineFourActive)
        {
            lineFour.SetActive(true);
            lineFourButton.SetActive(false);
            lineFiveButton.SetActive(true);
        }
    }

    public void LineFiveActivate()
    {
        lineFiveActive = true;
        if (lineFiveActive)
        {
            lineFive.SetActive(true);
            lineFiveButton.SetActive(false);
            cLineOneButton.SetActive(true);
        }
    }

    //for controls text
    public void cLineOneActivate()
    {
        cLineOneActive = true;
        if (lineFiveActive)
        {
            storyTextCanvas.SetActive(false);
            controlsTextCanvas.SetActive(true);
        }
        if (cLineOneActive)
        {
            cLineOne.SetActive(true);
            moveControls.SetActive(true);
            cLineOneButton.SetActive(false);
            cLineTwoButton.SetActive(true);
        }
    }

    public void cLineTwoActivate()
    {
        cLineTwoActive = true;
        if (cLineTwoActive)
        {
            cLineTwo.SetActive(true);
            dashControls.SetActive(true);
            cLineTwoButton.SetActive(false);
            cLineThreeButton.SetActive(true);
        }
    }

    public void cLineThreeActivate() 
    {
        cLineThreeActive = true;
        if (cLineThreeActive)
        {
            cLineThree.SetActive(true);
            interactControls.SetActive(true);
            cLineThreeButton.SetActive(false);
            cLineFourButton.SetActive(true);
        }
    }

    public void cLineFourActivate()
    {
        cLineFourActive = true;
        if (cLineFourActive)
        {
            cLineFour.SetActive(true);
            attackControls.SetActive(true);
            cLineFourButton.SetActive(false);
            cLineFiveButton.SetActive(true);
        }
    }

    public void cLineFiveActivate()
    {
        cLineFiveActive = true;
        if (cLineFiveActive)
        {
            cLineFive.SetActive(true);
            pauseControls.SetActive(true);
            cLineFourButton.SetActive(false);
            cLineFiveButton.SetActive(true);
            goToGameText.SetActive(true);
            goToGameButton.SetActive(true);
        }
    }

    public void GoToGame()
    {
        SceneManager.LoadScene("FirstLevel");
    }
}
