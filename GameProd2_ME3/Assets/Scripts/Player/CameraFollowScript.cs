using UnityEngine;

//the script to make the camera follow the player
public class CameraFollowScript : MonoBehaviour
{
    //the speed at which the camera follows the player
    [SerializeField] private float followSpeed = 0.1f;

    //to make sure the camera actually sees the player sprite
    [SerializeField] private Vector3 offset;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = Vector3.Lerp(transform.position, PlayerController.instance.transform.position + offset, followSpeed);
    }
}
