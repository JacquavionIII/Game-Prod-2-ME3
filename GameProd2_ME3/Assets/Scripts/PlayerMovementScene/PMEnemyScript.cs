using UnityEngine;

public class PMEnemyScript : MonoBehaviour
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

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    protected virtual void Start()
    {
        
    }
    protected virtual void Awake()
    {
        enemyRB = GetComponent<Rigidbody2D>();
        playerController = PlayerController.instance;
    }

    // Update is called once per frame
    protected virtual void Update()
    {
        if (health <= 0)
        {
            Destroy(gameObject);
        }
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
    }

    public virtual void EnemyHit(float _damageDone, Vector2 _hitDirection, float _hitForce)
    {
        health -= _damageDone;
        if (!isRecoiling)
        {
            enemyRB.AddForce(-_hitForce * recoilFactor * _hitDirection);
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
