using UnityEngine;

public class BatEnemyScript : EnemyScript
{
    [SerializeField] private float chaseDistance;

    [SerializeField] private float stunDuration;

    float timer;

    protected override void Start()
    {
        base.Start();
        ChangeState(EnemyStates.Bat_Idle);
    }

    protected override void UpdateEnemyStates()
    {
        float _dist = Vector2.Distance(transform.position, PlayerController.instance.transform.position);

        switch (GetCurrentEnemyState)
        {
            case EnemyStates.Bat_Idle:
                if (_dist < chaseDistance)
                {
                    ChangeState(EnemyStates.Bat_Chase);
                }
            break;
            case EnemyStates.Bat_Chase:
                enemyRB.MovePosition(Vector2.MoveTowards(transform.position, PlayerController.instance.transform.position, Time.deltaTime * Speed));

                FlipBat();
            break;
            case EnemyStates.Bat_Stunned:
                timer += Time.deltaTime;

                if (timer > stunDuration)
                {
                    ChangeState(EnemyStates.Bat_Idle);
                    timer = 0;
                }
            break;
            case EnemyStates.Bat_Death:
                Death(Random.Range(5, 10));
            break;
        }
    }

    protected override void Death(float _destroyTime)
    {
        enemyRB.gravityScale = 12;
        base.Death(_destroyTime);
    }

    public override void EnemyHit(float _damageDone, Vector2 _hitDirection, Vector2 _recoilDir, float _hitForce)
    {
        base.EnemyHit(_damageDone, _hitDirection, _recoilDir, _hitForce);

        if (health > 0)
        {
            ChangeState(EnemyStates.Bat_Stunned);
        }
        else
        {
            ChangeState(EnemyStates.Bat_Death);
        }
    }

    protected override void ChangeCurrentAnimation()
    {
        enemyAnim.SetBool("Idle", GetCurrentEnemyState == EnemyStates.Bat_Idle);
        enemyAnim.SetBool("Chase", GetCurrentEnemyState == EnemyStates.Bat_Chase);
        enemyAnim.SetBool("Stunned", GetCurrentEnemyState == EnemyStates.Bat_Stunned);

        if (GetCurrentEnemyState == EnemyStates.Bat_Death)
        {
            enemyAnim.SetTrigger("Death");
        }
    }

    void FlipBat()
    {
        if (PlayerController.instance.transform.position.x < transform.position.x)
        {
            enemySR.flipX = false;
        }
        else
        {
           enemySR.flipX = true;          
        }
    }
}
