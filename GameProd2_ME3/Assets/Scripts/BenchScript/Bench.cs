using UnityEngine;
using UnityEngine.InputSystem;

public class Bench : MonoBehaviour
{
    public bool benchInteracted;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerStay2D(Collider2D _other)
    {
        if (_other.CompareTag("Player") && Input.GetButtonDown("Interacted"))
        {
            Debug.Log("Bench interacted with");
            benchInteracted = true;
        }
    }

    private void OnTriggerExit2D(Collider2D _other)
    {
        if (_other.CompareTag("Player"))
        {
            benchInteracted = false;
        }
    }
}
