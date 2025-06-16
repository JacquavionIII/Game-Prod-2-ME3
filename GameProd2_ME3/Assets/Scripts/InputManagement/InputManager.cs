using UnityEngine;
using UnityEngine.InputSystem;

public class InputManager : MonoBehaviour
{
    public static InputManager instance;

    [HideInInspector] public InputSystem_Actions controls;
    [HideInInspector] public PlayerInput playerInput;

    private void Awake()
    {
        if (instance == null) 
        {
            instance = this;
        }

        controls = new InputSystem_Actions();

        playerInput = GetComponent<PlayerInput>();
    }

    private void OnEnable()
    {
        controls.Enable();
    }

    private void OnDisable()
    {
        controls.Disable();
    }
}
