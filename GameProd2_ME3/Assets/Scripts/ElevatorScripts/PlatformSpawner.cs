using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//this is the spawning script of the platforms
public class PlatformSpawner : MonoBehaviour
{
    //References:

    //Title: The Unity Tutorial For Complete Beginners
    //Author: G.M.T.
    //Date: 2022, December 2
    //Code version: Unity 2022.3.39f1
    //Availability: https://youtu.be/XtQMytORBmM?si=Yp5YSKFqfEqR83yk

    //Variables:
    public GameObject platform;//the variable for the platform prefab game object
    public float spawnRate;//the variable for the rate at which the platforms spawn
    private float timer = 0;//this is to set a time limit between the spawn rate of the platforms between each other

    // Start is called before the first frame update
    void Start()
    {
        spawnPlatform();//calls the method just before the first frame
    }

    // Update is called once per frame
    void Update()
    {
        //the if statement checks if the timer is less than the spawn rate, then it adds to the timer according to the time of the player's computer
        if (timer < spawnRate)
        {
            timer = timer + Time.deltaTime;
        }
        else//this then means that if the timer is more or equal to the spawn rate, then it spawns the platform and puts the timer at 0 again
        {
            spawnPlatform();
            timer = 0;
        }
    }

    //this is the method for spawning the platforms
    void spawnPlatform()
    {
        Instantiate(platform, new Vector3(7, -6, 0), transform.rotation);//spawns the platform at the designated point
    }
}
