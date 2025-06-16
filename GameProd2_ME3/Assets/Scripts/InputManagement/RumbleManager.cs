using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class RumbleManager : MonoBehaviour
{
    public static RumbleManager instance;

    private Gamepad gamepad;

    private Coroutine stopRumbleAfterTimeCoroutine;

    private string currentControlScheme;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
    }

    private void Start()
    {
        InputManager.instance.playerInput.onControlsChanged += SwitchControls;
    }

    //most gamepads have two motors, the left and right. Left is usually a low frequency,
    //right is usually a high frequency. Setting float values to set those frequencies below
    public void RumblePulse(float lowFrequency, float highFrequency, float duration)
    {
        //is our current gamepad
        if (currentControlScheme == "Gamepad")
        {
            //get reference to the gamepad
            gamepad = Gamepad.current;

            //if we have a current gamepad
            if (gamepad != null)
            {
                //start rumble
                gamepad.SetMotorSpeeds(lowFrequency, highFrequency);

                //stop the rumble after certain time
                stopRumbleAfterTimeCoroutine = StartCoroutine(StopRumble(duration, gamepad));
            }
        }
    }

    private IEnumerator StopRumble(float rumbleDuration, Gamepad pad)
    {
        //this will rumble as long as every frame is according to specific time
        float elapsedTime = 0f;
        while (elapsedTime < rumbleDuration)
        {
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        //once our duration is over
        pad.SetMotorSpeeds(0f, 0f);
    }

    private void SwitchControls(PlayerInput input)
    {
        //Debug.Log("Device is now: " + input.currentControlScheme);
        currentControlScheme = input.currentControlScheme;
    }

    private void OnDisable()
    {
        InputManager.instance.playerInput.onControlsChanged -= SwitchControls;
    }
}
