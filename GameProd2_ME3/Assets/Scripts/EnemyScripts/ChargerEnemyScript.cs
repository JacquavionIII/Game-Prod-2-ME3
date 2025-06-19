using UnityEngine;

public class ChargerEnemyScript : EnemyScript
{
    [SerializeField] private float ledgeCheckX;
    [SerializeField] private float ledgeCheckY;
    [SerializeField] private float chargeSpeedMultiplier;
    [SerializeField] private float chargeDuration;
    [SerializeField] private float jumpForce;
    [SerializeField] private LayerMask whatIsGround;

    float timer;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    protected override void Start()
    {
        base.Start();
        ChangeState(EnemyStates.Charger_Idle);
        enemyRB.gravityScale = 12f;
    }

    protected override void UpdateEnemyStates()
    {
        if (health <= 0)
        {
            Death(0.05f);
        }

        Vector3 _ledgeCheckStart = transform.localScale.x > 0 ? new Vector3(ledgeCheckX, 0) : new Vector3(-ledgeCheckX, 0);
        Vector2 _wallCheckDir = transform.localScale.x > 0 ? transform.right : -transform.right;

        switch (GetCurrentEnemyState)
        {
            case EnemyStates.Charger_Idle:

                if (!Physics2D.Raycast(transform.position + _ledgeCheckStart, Vector2.down, ledgeCheckY, whatIsGround)
                    || Physics2D.Raycast(transform.position, _wallCheckDir, ledgeCheckX, whatIsGround))
                {
                    transform.localScale = new Vector2(transform.localScale.x * -1, transform.localScale.y);
                }

                RaycastHit2D _hit = Physics2D.Raycast(transform.position + _ledgeCheckStart, _wallCheckDir, ledgeCheckX * 10);
                if (_hit.collider != null && _hit.collider.gameObject.CompareTag("Player"))
                {
                    ChangeState(EnemyStates.Charger_Surprised);
                }

                if (transform.localScale.x > 0)
                {
                    enemyRB.linearVelocity = new Vector2(Speed, enemyRB.linearVelocity.y);
                }
                else
                {
                    enemyRB.linearVelocity = new Vector2(-Speed, enemyRB.linearVelocity.y);
                }
                break;

            case EnemyStates.Charger_Surprised:
                enemyRB.linearVelocity = new Vector2(0, jumpForce);

                ChangeState(EnemyStates.Charger_Charge);
            break;

            case EnemyStates.Charger_Charge:
                timer += Time.deltaTime;

                if (timer < chargeDuration)
                {
                    if (Physics2D.Raycast(transform.position, Vector2.down, ledgeCheckY, whatIsGround))
                    {
                        if (transform.localScale.x > 0)
                        {
                            enemyRB.linearVelocity = new Vector2(Speed * chargeSpeedMultiplier, enemyRB.linearVelocity.y);
                        }
                        else
                        {
                            enemyRB.linearVelocity = new Vector2(-Speed * chargeSpeedMultiplier, enemyRB.linearVelocity.y);
                        }
                    }
                    else
                    {
                        enemyRB.linearVelocity = new Vector2(0, enemyRB.linearVelocity.y);
                    }
                }
                else
                {
                    timer = 0;
                    ChangeState(EnemyStates.Charger_Idle);
                }
                
            break;
        }
    }

    protected override void ChangeCurrentAnimation()
    {
        if (GetCurrentEnemyState == EnemyStates.Charger_Idle)
        {
            enemyAnim.speed = 1;
        }

        if (GetCurrentEnemyState == EnemyStates.Charger_Charge)
        {
            enemyAnim.speed = chargeSpeedMultiplier;
        }
    }
}
