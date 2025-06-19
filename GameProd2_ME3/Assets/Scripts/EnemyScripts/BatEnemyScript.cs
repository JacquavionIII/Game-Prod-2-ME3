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

        switch (currentEnemyState)
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
                
            break;
        }
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

    void FlipBat()
    {
        if (PlayerController.instance.transform.position.x < transform.position.x)
        {
            enemySR.flipX = true;
        }
        else
        {
           enemySR.flipX = false;          
        }
    }
}
