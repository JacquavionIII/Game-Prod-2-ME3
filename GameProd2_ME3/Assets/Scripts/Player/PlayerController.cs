using UnityEngine;

//the player movement script to walk/attack
public class PlayerController : MonoBehaviour
{
    [Header("Horizontal Movement Settings")]
    //the player rigid body
    [SerializeField] private Rigidbody2D playerRB;
    //the player's walk speed
    [SerializeField] private float walkSpeed = 1;
    //getting player input as a float
    private float xAxis;

    //a variable for the camera script to track the player
    public static PlayerController instance;

    private void Awake()
    {
        //to ensure there aren't any other players
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            instance = this;
        }
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        GetInputs();
        Move();
        Flip();
    }

    //getting the input on the horizontal plane from the player
    void GetInputs()
    {
        xAxis = Input.GetAxisRaw("Horizontal");
    }

    //making the player move according to the player's input
    private void Move()
    {
        playerRB.linearVelocity = new Vector2(walkSpeed * xAxis, playerRB.linearVelocity.y);
    }

    //flipping the sprite corresponding to the direction(will be helpful when actual sprites are added
    void Flip()
    {
        if (xAxis < 0)
        {
            transform.localScale = new Vector2(-1, transform.localScale.y);
        }
        else if (xAxis > 0)
        {
            transform.localScale = new Vector2(1, transform.localScale.y);
        }
    }
}
