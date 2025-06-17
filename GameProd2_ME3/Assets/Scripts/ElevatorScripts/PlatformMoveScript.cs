using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//this script is for the platforms that are being spawned to move of their own accord
public class PlatformMoveScript : MonoBehaviour
{
    //References:

    //Title: The Unity Tutorial For Complete Beginners
    //Author: G.M.T.
    //Date: 2022, December 2
    //Code version: Unity 2022.3.39f1
    //Availability: https://youtu.be/XtQMytORBmM?si=Yp5YSKFqfEqR83yk

    //Variables:
    public float moveSpeed = 5;//the variable for the moving speed of the platforms
    public float deadZone = 6;//the variable for where the game object should be destroyed to save memory

    // Update is called once per frame
    void Update()
    {
        //this is to move the platform prefab object so that it moves up according to the player's computer or device
        transform.position = transform.position + (Vector3.up * moveSpeed) * Time.deltaTime;
        
        /* This was the previous way I would check the deadzone for the game object, and switched to using an array to store the game objects, refer to ArrayPlatformHandler script
         * 
         * //this if statement checks if the position of the game object this script is attached to if it's above the deadzone float number
        if (transform.position.y > deadZone)
        {
            //Debug.Log("Platform deleted");
            Destroy(gameObject);//destroys the game object this script is attached to
        }*/
    }

    //The method for the deadzone of the game object to delete the game object to save memory
    public void CheckDeadZone()
    {
        //this if statement checks if the position of the game object this script is attached to if it's above the deadzone float number
        if (transform.position.y > deadZone)
        {
            //Debug.Log("Platform deleted");
            Destroy(gameObject);//destroys the game object this script is attached to
        }
    }
}
