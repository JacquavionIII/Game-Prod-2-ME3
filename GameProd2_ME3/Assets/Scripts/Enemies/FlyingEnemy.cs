using UnityEngine;

public class FlyingEnemy : MonoBehaviour
{
    [SerializeField] float moveSpeed;
    [SerializeField] Vector2 moveDirection = new Vector2(1f, 0.25f);
    [SerializeField] GameObject rightCheck, roofCheck, groundCheck;
    [SerializeField] Vector2 rightCheckSize, roofCheckSize, groundCheckSize;
    [SerializeField] LayerMask groundLayer, platform;
    [SerializeField] bool goingUp = true;

    private bool touchedGround, touchedRoof, touchedRight;
    private Rigidbody2D EnemyRB;

    void Start()
    {
        EnemyRB = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        HitLogic();
    }

    void FixedUpdate()
    {
        EnemyRB.linearVelocity = moveDirection * moveSpeed;
    }

    void HitLogic()
    {
        touchedRight = HitDetector(rightCheck, rightCheckSize, (groundLayer | platform)); //the brackets hold our layers that are being checked witht the bools
        touchedRoof = HitDetector(roofCheck, roofCheckSize, (groundLayer | platform));
        touchedGround = HitDetector(groundCheck, groundCheckSize, (groundLayer | platform));

        if (touchedRight) //so that the enemy flps whenever the right check hits the right layer
        {
            Flip();
        }
        if (touchedRoof && goingUp) //if the enemy hits the roof and is going up, it will change direction
        {
            ChangeYDirection();
        }
        {
            ChangeYDirection();
        }
        if (touchedGround && !goingUp) //if the enemy hits the ground amd is not going up, it will change direction
        {
            ChangeYDirection();
        }
    }

    bool HitDetector(GameObject gameObject, Vector2 size, LayerMask layer)
    {
        return Physics2D.OverlapBox(gameObject.transform.position, size, 0f, layer); //Using the gameobjects position, HitDectector checks if the box overlaps with the layer mask
    }

    void ChangeYDirection()
    {
        moveDirection.y = -moveDirection.y; //makes it negative to change the y direction of the enemy
        goingUp = !goingUp; //makes the goingUp bool false so that it can change direction again when it hits the roof or ground
    }

    void Flip()
    {
        transform.Rotate(new Vector2(0, 180));//flips the enemy by rotating it 180 degrees on the y-axis
        moveDirection.x = -moveDirection.x; //makes the movedirection negative to change the direction it's moving in
    }

    private void OnDrawGizmosSelectred() // Draws the gizmos in the editor to visualize the checks
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireCube(groundCheck.transform.position, groundCheckSize);
        Gizmos.DrawWireCube(roofCheck.transform.position, roofCheckSize);
        Gizmos.DrawWireCube(rightCheck.transform.position, rightCheckSize);
    }


}
