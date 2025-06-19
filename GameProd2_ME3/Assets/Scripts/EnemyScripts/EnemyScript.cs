using UnityEngine;

public class EnemyScript : MonoBehaviour
{
    [SerializeField] protected float health;
    [SerializeField] protected float recoilLength;
    [SerializeField] protected float recoilFactor;
    [SerializeField] protected bool isRecoiling = false;

    [SerializeField] protected PlayerController playerController;
    [SerializeField] protected float Speed;

    [SerializeField] protected float enemyDamage;
    [SerializeField] protected GameObject enemyBlood;

    protected float recoilTimer;
    protected Rigidbody2D enemyRB;
    protected SpriteRenderer enemySR;
    protected Animator enemyAnim;

    protected enum EnemyStates
    {
        //crawler
        Crawler_Idle,
        Crawler_Flip,

        //bat
        Bat_Idle,
        Bat_Chase,
        Bat_Stunned,
        Bat_Death,

        //Charger
        Charger_Idle,
        Charger_Surprised,
        Charger_Charge

    }

    protected EnemyStates currentEnemyState;

    protected virtual EnemyStates GetCurrentEnemyState
    {
        get { return currentEnemyState; }
        set
        {
            if (currentEnemyState != value)
            {
                currentEnemyState = value;

                ChangeCurrentAnimation();
            }
        }
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    protected virtual void Start()
    {
        enemyRB = GetComponent<Rigidbody2D>();
        playerController = PlayerController.instance;
        enemySR = GetComponent<SpriteRenderer>();
        enemyAnim = GetComponent<Animator>();
    }

    // Update is called once per frame
    protected virtual void Update()
    {
        if (isRecoiling)
        {
            if (recoilTimer < recoilLength)
            {
                recoilTimer += Time.deltaTime;
            }
            else
            {
                isRecoiling = false;
                recoilTimer = 0;
            }
        }
        else
        {
            UpdateEnemyStates();
        }
    }

    public virtual void EnemyHit(float _damageDone, Vector2 _hitDirection, Vector2 _recoilDir, float _hitForce)
    {
        health -= _damageDone;
        if (!isRecoiling)
        {
            GameObject _enemyBlood = Instantiate(enemyBlood, transform.position, Quaternion.identity);
            Destroy(_enemyBlood, 5.5f);
            enemyRB.linearVelocity = -_hitForce * recoilFactor * _hitDirection;
            isRecoiling = true;
        }
        if (isRecoiling )
        {
            if (recoilTimer < recoilLength)
            {
                recoilTimer += Time.deltaTime;
            }
            else
            {
                isRecoiling = false;
                recoilTimer = 0;
            }
        }
    }

    protected virtual void Death(float _destroyTime)
    {
        Destroy(gameObject, _destroyTime);
    }

    protected virtual void UpdateEnemyStates()
    {

    }

    protected virtual void ChangeCurrentAnimation()
    {

    }

    protected void ChangeState(EnemyStates _newState)
    {
        GetCurrentEnemyState = _newState;
    }

    protected void OnCollisionStay2D(Collision2D _other)
    {
        if (_other.gameObject.CompareTag("Player") && !PlayerController.instance.pState.invincibleFrames && health > 0)
        {
            Attack();
            PlayerController.instance.HitStopTime(0, 5, 0.5f);
        }
    }

    protected virtual void Attack()
    {
        PlayerController.instance.TakeDamage(enemyDamage);
    }
}
