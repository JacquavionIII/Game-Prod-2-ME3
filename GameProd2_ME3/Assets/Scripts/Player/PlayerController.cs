using NUnit.Framework;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;

//the player movement script to walk/attack
public class PlayerController : MonoBehaviour
{
    [Header("Input System")]
    [SerializeField] public InputActionAsset inputActions;

    [Header("Input System Actions")]
    private InputAction m_moveAction;
    private InputAction m_attackAction;
    private InputAction m_interactAction;
    private InputAction m_dashAction;
    private InputAction m_healAction;

    private Vector2 m_moveAmt;

    [Header("Horizontal Movement Settings")]
    //the player rigid body
    [SerializeField] private Rigidbody2D playerRB;
    //the player's walk speed
    [SerializeField] private float walkSpeed = 1;
    //getting player input as a float
    private float xAxis, yAxis;

    //the animator controller
    [SerializeField] private Animator anim;

    [Header("Ground Check Settings")]
    //to check when the player is on ground
    [SerializeField] private Transform groundCheckPoint;

    [SerializeField] private float groundCheckY = 0.2f;
    [SerializeField] private float groundCheckX = 0.5f;

    [SerializeField] private LayerMask whatIsGround;

    //a variable for the camera script to track the player
    public static PlayerController instance;

    //to access the playerstatelist script
    [SerializeField] public PlayerStateList pState;

    [Space(5)]

    [Header("Dash settings")]
    //dash variables
    [SerializeField] private float dashSpeed;
    [SerializeField] private float dashTime;
    [SerializeField] private float dashCooldown;

    [Space(5)]

    [Header("Attacking")]
    bool attack = false;
    [SerializeField] private float timeBetweenAttack;
    private float timeSinceAttack;
    [SerializeField] Transform sideAttackTransform, upAttackTransform, downAttackTransform;
    [SerializeField] Vector2 sideAttackArea, upAttackArea, downAttackArea;
    [SerializeField] LayerMask attackableLayer;
    [SerializeField] float damage;//the amount of damage the player deals

    //this is to stop the game for a bit when hit to let the player bear what just happened and gain their surroundings
    bool restoreTime;
    float restoreTimeSpeed;

    //the below will be once the slashEffect animation is made
    // [SerializeField] GameObject slashEffect;
    [Space(5)]   

    [Header("Recoil")]
    // [SerializeField] int recoilXSteps = 5;
    // [SerializeField] int recoilYSteps = 5;
    [SerializeField] float recoilXSpeed = 100;
    [SerializeField] float recoilYSpeed = 100;
    [SerializeField] float enemyHitRecoilStrength;
    // int stepsXRecoiled, stepsYRecoiled;
    [Space(5)]

    [Header("Player Health Settings")]
    public int playerHealth;
    public int maxPlayerHealth;
    //this is for the particles of when the player is hit to signify they're hit(will be better once blood circle's created for the material)
    [SerializeField] GameObject bloodSpurt;
    //flashing the player sprite to signify they're invincible
    [SerializeField] float hitFlashSpeed;
    //responsible for any updates we may need with the hearts
    public delegate void OnHealthChangedDelegate();
    [HideInInspector] public OnHealthChangedDelegate onHealthChangedCallback;

    //limits the speed of healing
    float healTimer;
    [SerializeField] float timeToHeal;
    [Space(5)]

    //this will be the "mana" from hollow knight allowing the player to do actions(refer to part 4 of tutorials)
    [Header("Ability settings")]
    [SerializeField] Image abilityStorage;

    [SerializeField] float abilityLimit;
    [SerializeField] float abilityDrainSpeed;
    [SerializeField] float abilityGain;

    [Space(5)]

    [Header("Ability cast settings")]
    [SerializeField] float abilityDamage;
    [Space(5)]

    //the bool for if the player is dashing
    private bool canDash = true;
    //bool to make sure the player only dashes once and not continuously
    private bool dashed;

    //to keep reference to the rigid body's scale
    private float gravity;

    //to get access to the player sprite renderer
    [SerializeField] private SpriteRenderer sr;


    //the following is pertaining to activating the input system:

    private void OnEnable()
    {
        inputActions.FindActionMap("Player").Enable();
    }

    private void OnDisable()
    {
        inputActions.FindActionMap("Player").Disable();
    }

    private void Awake()
    {
        //getting the actions
        m_moveAction = InputSystem.actions.FindAction("Move");
        m_attackAction = InputSystem.actions.FindAction("Attack");
        m_interactAction = InputSystem.actions.FindAction("Interact");
        m_dashAction = InputSystem.actions.FindAction("Dash");
        m_healAction = InputSystem.actions.FindAction("Healing");

        //to ensure there aren't any other players
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            instance = this;
        }
        DontDestroyOnLoad(gameObject);

        PlayerHealth = maxPlayerHealth;
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        gravity = playerRB.gravityScale;
        AbilityLimit = abilityLimit;
        abilityStorage.fillAmount = AbilityLimit;
    }

    //this will just be creating the range for the attack spaces for the player
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(sideAttackTransform.position, sideAttackArea);
        Gizmos.DrawWireCube(upAttackTransform.position, upAttackArea);
        Gizmos.DrawWireCube(downAttackTransform.position, downAttackArea);
    }

    // Update is called once per frame
    void Update()
    {
        GetInputs();

        RestoreTimeScale();
        if (pState.dashing) return;
        Move();
        FlashWhileInvincible();
        Heal();

        if (pState.healing) return;
        Flip();
        StartDash();
        Attack();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.GetComponent<PMEnemyScript>() != null && pState.casting)
        {
            other.GetComponent<PMEnemyScript>().EnemyHit(abilityDamage, (other.transform.position - transform.position).normalized, -recoilYSpeed);
        }
    }

    private void FixedUpdate()
    {
        //if (pState.dashing) return;
        //Recoil();
    }

    //getting the input on the horizontal plane from the player
    void GetInputs()
    {
        xAxis = Input.GetAxisRaw("Horizontal");
        yAxis = Input.GetAxisRaw("Vertical");
        attack = m_attackAction.IsPressed();
    }

    //making the player move according to the player's input
    private void Move()
    {
        if (pState.healing) playerRB.linearVelocity = new Vector2(0, 0);
        playerRB.linearVelocity = new Vector2(walkSpeed * xAxis, playerRB.linearVelocity.y);
        //to go between the walking and idle animations
        anim.SetBool("isWalking", playerRB.linearVelocity.x != 0 && Grounded());
    }

    //take note, this might make the player dash in the air when not grounded,
    //so might need to go back to part 1 to set the grounded variable stuff
    void StartDash()
    {
        if (m_dashAction.WasPressedThisFrame() && canDash && !dashed)
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
        anim.SetTrigger("Dashing"); //the reference to the animator controller        

        playerRB.gravityScale = 0;
        int _dir = pState.lookingRight ? 1 : -1;
        playerRB.linearVelocity = new Vector2(_dir * dashSpeed, 0);
        yield return new WaitForSeconds(dashTime);
        playerRB.gravityScale = gravity;
        pState.dashing = false;
        yield return new WaitForSeconds(dashCooldown);
        canDash = true;
    }

    //the attack function
    void Attack()
    {
        timeSinceAttack += Time.deltaTime;
        if (attack && timeSinceAttack >= timeBetweenAttack)
        {
            Debug.Log("Attacking");
            timeSinceAttack = 0;
            //for the animation stuff when made
            anim.SetTrigger("Attacking");

            if (yAxis == 0 || yAxis < 0 && Grounded()) //if the player on the ground and attacking
            {
                Hit(sideAttackTransform, sideAttackArea, ref pState.recoilingX, recoilXSpeed);

                //with the slash effect animation prefab
                //Instantiate(slashEffect, sideAttackTransform);
            }
            else if (yAxis > 0) //if player holds W or Up
            {
                Hit(upAttackTransform, upAttackArea, ref pState.recoilingY, recoilYSpeed);

                //with the slash effect animation prefab
                //SlashEffectAtAngle(slashEffect, 80, upAttackTransform);
            }
            else if (yAxis < 0 && !Grounded()) //if player holds S or Down, more so for jumping
            {
                Hit(downAttackTransform, downAttackArea, ref pState.recoilingY, recoilYSpeed);

                //with the slash effect animation prefab
                //SlashEffectAtAngle(slashEffect, -90, downAttackTransform);
            }
        }
    }

    //when the player does hit an object
    void Hit(Transform _attackTransform, Vector2 _attackArea, ref bool recoilingX, float _recoilStrength /*ref bool _recoilDir,*/)
    {
        Collider2D[] objectsToHit = Physics2D.OverlapBoxAll(_attackTransform.position, _attackArea, 0, attackableLayer);
        List<PMEnemyScript> hitEnemies = new List<PMEnemyScript>();

        if (objectsToHit.Length > 0)
        {
            Debug.Log("Hit object");
            //_recoilDir = true;
        }
        for (int i = 0; i < objectsToHit.Length; i++) //detecting if an enemy is in the range of the attack
        {
            PMEnemyScript e = objectsToHit[i].GetComponent<PMEnemyScript>();
            if (e && !hitEnemies.Contains(e))
            {
                e.EnemyHit(damage, (transform.position - objectsToHit[i].transform.position).normalized, _recoilStrength);
                hitEnemies.Add(e);

                if (objectsToHit[i].CompareTag("Enemy"))
                {
                    AbilityLimit += abilityGain;
                }
            }
        }
    }

    //this will be when the slash effect is made
    /*
    void SlashEffectAtAngle(GameObject _slashEffect, int _effectAngle, Transform _attackTransform)
    {
        _slashEffect = Instantiate(_slashEffect, _attackTransform);
        _slashEffect.transform.eulerAngles = new Vector3(0, 0, _effectAngle);
        _slashEffect.transform.localScale = new Vector2(transform.localScale.x, transform.localScale.y);
    }
    */

    //the below would be if we wanted to recoil the player back when hitting an enemy

    /*

    void Recoil()
    {
        //to recoil the player based on the direction they hit an enemy
        if (pState.recoilingX)
        {
            if (pState.lookingRight)
            {
                playerRB.linearVelocity = new Vector2(-recoilXSpeed, 0);
            }
            else
            {
                playerRB.linearVelocity = new Vector2(recoilXSpeed, 0);
            }
        }

        //this part is more for when attacking downwards or up when jumping
        if (pState.recoilingY)
        {
            playerRB.gravityScale = 0;
            if (yAxis < 0)
            {
                playerRB.linearVelocity = new Vector2(playerRB.linearVelocity.x, recoilYSpeed);
            }
            else
            {
                playerRB.linearVelocity = new Vector2(playerRB.linearVelocity.x, -recoilYSpeed);
            }
            //airJumpCounter = 0;
        }
        else
        {
            playerRB.gravityScale = gravity;
        }

        //stop recoil
        if (pState.recoilingX && stepsXRecoiled < recoilXSteps)
        {
            stepsXRecoiled++;
        }
        else
        {
            StopRecoilX();
        }
        if (pState.recoilingY && stepsYRecoiled < recoilYSteps)
        {
            stepsYRecoiled++;
        }
        else
        {
            StopRecoilY();
        }

        if (Grounded())
        {
            StopRecoilY();
        }
    }

    void StopRecoilX()
    {
        stepsXRecoiled = 0;
        pState.recoilingX = false;
    }

    void StopRecoilY()
    {
        stepsYRecoiled = 0;
        pState.recoilingY = false;
    }

    */

    public void TakeDamage(float _damage)
    {
        PlayerHealth -= Mathf.RoundToInt(_damage);
        StartCoroutine(StopTakingDamage());
    }

    IEnumerator StopTakingDamage()
    {
        pState.invincibleFrames = true;
        GameObject _bloodSpurtParticles = Instantiate(bloodSpurt, transform.position, Quaternion.identity);
        Destroy(_bloodSpurtParticles, 1.5f);
        anim.SetTrigger("TakeDamage");
        yield return new WaitForSeconds(1f);
        pState.invincibleFrames = false;
    }

    void FlashWhileInvincible()
    {
        sr.material.color = pState.invincibleFrames ? 
            Color.Lerp(Color.white, Color.black, Mathf.PingPong(Time.time * hitFlashSpeed, 1.0f)) : Color.white;
    }

    void RestoreTimeScale()
    {
        if (restoreTime)
        {
            if (Time.timeScale < 1)
            {
                Time.timeScale += Time.unscaledDeltaTime * restoreTimeSpeed;
            }
            else
            {
                Time.timeScale = 1f;
                restoreTime = false;
            }
        }
    }

    public void HitStopTime(float _newTimeScale, int _restoreSpeed, float _delay)
    {
        restoreTimeSpeed = _restoreSpeed;
        Time.timeScale = _newTimeScale;

        if (_delay > 0)
        {
            StopCoroutine(StartTimeAgain(_delay));
            StartCoroutine(StartTimeAgain(_delay));
        }
        else
        {
            restoreTime = true;
        }
    }

    IEnumerator StartTimeAgain(float _delay)
    {
        yield return new WaitForSecondsRealtime(_delay);
        restoreTime = true;
        
    }

    public int PlayerHealth
    {
        get { return playerHealth; }
        set
        {
            if (playerHealth != value)
            {
                playerHealth = Mathf.Clamp(value, 0, maxPlayerHealth);

                if (onHealthChangedCallback != null)
                {
                    onHealthChangedCallback.Invoke();
                }
            }
        }
    }

    void Heal()
    {
        if (Input.GetButton("Healing") && PlayerHealth < maxPlayerHealth && AbilityLimit > 0 && Grounded() && !pState.dashing)
        {
            Debug.Log("Heal button pressed");
           pState.healing = true;
            anim.SetBool("isHealing", true);

            //healing
            healTimer += Time.deltaTime;
            if (healTimer >= timeToHeal)
            {
                PlayerHealth++;
                healTimer = 0;
            }

            //drain ability limit
            AbilityLimit -= Time.deltaTime * abilityDrainSpeed;
        }
        else
        {
            pState.healing = false;
            anim.SetBool("isHealing", false);
            healTimer = 0;
        }
    }


    //to keep a limit to how much the player can do
    float AbilityLimit
    {
        get { return abilityLimit; }
        set
        {
            //if ability limits change
            if (abilityLimit != value)
            {
                abilityLimit = Mathf.Clamp(value, 0, 1);
                abilityStorage.fillAmount = AbilityLimit;
            }
        }
    }

    //flipping the sprite corresponding to the direction(will be helpful when actual sprites are added
    void Flip()
    {
        if (xAxis < 0)
        {
            transform.localScale = new Vector2(-1, transform.localScale.y);
            pState.lookingRight = false;
        }
        else if (xAxis > 0)
        {
            transform.localScale = new Vector2(1, transform.localScale.y);
            pState.lookingRight = true;
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
