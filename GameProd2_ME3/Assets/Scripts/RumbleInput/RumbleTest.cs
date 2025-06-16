using Unity.VisualScripting;
using UnityEngine;

//this will be where everything that the player can do that needs rumble will be given rumble

public class RumbleTest : MonoBehaviour
{
    [SerializeField] private PlayerController playerController;

    private void Update()
    {
        //grabbed the "Player" action map from the Unity input system

        if (InputManager.instance.controls.Player.Dash.WasPressedThisFrame())
        {
            //call the rumble function
            RumbleManager.instance.RumblePulse(0.25f, 1f, 0.25f);
        }

        //for attacking
        if (InputManager.instance.controls.Player.Attack.WasPressedThisFrame())
        {
            RumbleManager.instance.RumblePulse(0.15f, 0.75f, 0.15f);
        }
    }
}
