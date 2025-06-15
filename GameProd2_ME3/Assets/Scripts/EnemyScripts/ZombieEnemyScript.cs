using UnityEngine;

public class ZombieEnemyScript : EnemyScript
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    protected override void Start()
    {
        base.Start();
        enemyRB.gravityScale = 12f;
    }

    // Update is called once per frame
    protected override void Update()
    {
        base.Update();
        if (!isRecoiling)
        {
            transform.position = Vector2.MoveTowards
                (transform.position, 
                new Vector2(PlayerController.instance.transform.position.x, PlayerController.instance.transform.position.y), 
                Speed * Time.deltaTime);
        }
    }

    public override void EnemyHit(float _damageDone, Vector2 _hitDirection, float _hitForce)
    {
        base.EnemyHit(_damageDone, _hitDirection, _hitForce);
    }
}
