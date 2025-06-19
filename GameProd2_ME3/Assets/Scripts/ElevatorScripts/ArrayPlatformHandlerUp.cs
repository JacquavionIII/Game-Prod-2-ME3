using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//this script is for the array management for the platforms
public class ArrayPlatformHandlerUp : MonoBehaviour
{
    //Reference:
    //Title: Unity Beginners - How to use Arrays 
    //Author: Adamant Algorithm
    //Date: 2020, May 14
    //Code version: Unity 2022.3.39f1
    //Availability: https://www.youtube.com/watch?v=nIWhX8w7NB4

    public GameObject[] platformArray = new GameObject[7];
    private GameObject platform;
    private PlatformMoveScript platformMoveScript;

    // Update is called once per frame
    void Update()
    {
        //this is to find the platform game object as it is instantiated

        platform = GameObject.FindGameObjectWithTag("PlatformUp");

        //this is to get the script to call the deadzone method from the PlatformMoveScript

        platformMoveScript = GameObject.FindGameObjectWithTag("PlatformUp").GetComponent<PlatformMoveScript>();

        //this is the line so that it stores the platform object into the array

        platformArray = GameObject.FindGameObjectsWithTag("PlatformUp");

        //the for loop checks if it's less than the length, then it will go through the deadzone method and destroy the game object

        for (int i = 0; i < platformArray.Length; i++)
        {
            platformMoveScript.CheckDeadZoneUp();//the checkdeadzoneUp method from the PlatformMoveScript
        }
    }
}
