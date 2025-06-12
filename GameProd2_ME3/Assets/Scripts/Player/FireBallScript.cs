using UnityEngine;


//the script for the fireball object
public class FireBallScript : MonoBehaviour
{
    [SerializeField] float damage;
    [SerializeField] float hitForce;
    [SerializeField] int speed;
    [SerializeField] float lifeTime = 1f;//1 second passed

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Destroy(gameObject, lifeTime);
    }

    private void FixedUpdate()
    {
        transform.position += speed * transform.right;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Enemy")
        {
            other.GetComponent<PMEnemyScript>().EnemyHit(damage, (other.transform.position - transform.position).normalized, -hitForce);
        }
    }
}
