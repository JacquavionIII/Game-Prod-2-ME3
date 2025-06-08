using UnityEngine;


//this script is to ensure the enemy stays on the platform before falling onto the other platforms
public class ZombieColliderScript : MonoBehaviour
{
    [SerializeField] private GameObject zombieCollider;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            zombieCollider.SetActive(false);
        }
    }
}
