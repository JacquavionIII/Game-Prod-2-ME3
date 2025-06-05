using System.Collections;
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

    [Header("Ground Check Settings")]
    //to check when the player is on ground
    [SerializeField] private Transform groundCheckPoint;

    [SerializeField] private float groundCheckY = 0.2f;
    [SerializeField] private float groundCheckX = 0.5f;

    [SerializeField] private LayerMask whatIsGround;

    //a variable for the camera script to track the player
    public static PlayerController instance;

    //to access the playerstatelist script
    [SerializeField] private PlayerStateList pState;

    [Header("Dash settings")]
    //dash variables
    [SerializeField] private float dashSpeed;
    [SerializeField] private float dashTime;
    [SerializeField] private float dashCooldown;

    //the bool for if the player is dashing
    private bool canDash = true;
    //bool to make sure the player only dashes once and not continuously
    private bool dashed;

    //to keep reference to the rigid body's scale
    private float gravity;

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
        gravity = playerRB.gravityScale;
    }

    // Update is called once per frame
    void Update()
    {
        GetInputs();

        if (pState.dashing) return;
        Move();
        Flip();
        StartDash();
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

    //take note, this might make the player dash in the air when not grounded,
    //so might need to go back to part 1 to set the grounded variable stuff
    void StartDash()
    {
        if (Input.GetButtonDown("Dash") && canDash && !dashed)
        {
            Debug.Log("Dash is pressed");
            StartCoroutine(Dash());
            dashed = true;
        }

        //this will be for when the player is touching the ground
        if (Grounded())
        {
            dashed = false;
        }
    }

    IEnumerator Dash()
    {
        canDash = false;

        //The following is for when the animations come into play
        pState.dashing = true; //this is from the pState script made in part 2 of the tutorial series
        //anim.SetTrigger("Dashing"); //the reference to the animator controller        

        playerRB.gravityScale = 0;
        playerRB.linearVelocity = new Vector2(transform.localScale.x * dashSpeed, 0);
        yield return new WaitForSeconds(dashTime);
        playerRB.gravityScale = gravity;
        pState.dashing = false;
        yield return new WaitForSeconds(dashCooldown);
        canDash = true;
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

    public bool Grounded()
    {
        if (Physics2D.Raycast(groundCheckPoint.position, Vector2.down, groundCheckY, whatIsGround) || 
            Physics2D.Raycast(groundCheckPoint.position + new Vector3(groundCheckX, 0, 0), Vector2.down, groundCheckY, whatIsGround) ||
            Physics2D.Raycast(groundCheckPoint.position + new Vector3(-groundCheckX, 0, 0), Vector2.down, groundCheckY, whatIsGround))
        {
            return true;
        }
        else
        {
            return false;
        }
    }
}
