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

    protected float recoilTimer;
    protected Rigidbody2D enemyRB;
    protected SpriteRenderer enemySR;

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

    }

    protected EnemyStates currentEnemyState;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    protected virtual void Start()
    {
        enemyRB = GetComponent<Rigidbody2D>();
        playerController = PlayerController.instance;
        enemySR = GetComponent<SpriteRenderer>();
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

    protected virtual void UpdateEnemyStates()
    {

    }

    protected void ChangeState(EnemyStates _newState)
    {
        currentEnemyState = _newState;
    }

    protected void OnCollisionStay2D(Collision2D _other)
    {
        if (_other.gameObject.CompareTag("Player") && !PlayerController.instance.pState.invincibleFrames)
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
