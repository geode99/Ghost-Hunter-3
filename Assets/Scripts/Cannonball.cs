using UnityEngine;

public class Cannonball : MonoBehaviour
{
    public Rigidbody2D rb;

    void Update()
    {
           
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Wall"))
        {
            Destroy(gameObject);
        }

        if (other.CompareTag("Enemy"))
        {
            other.gameObject.GetComponent<EnemyHealth>().Health -= 2;
            Destroy(gameObject);
        }
    }
}