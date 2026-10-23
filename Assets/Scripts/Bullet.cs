using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float speed = 10f;
    public Rigidbody2D rb;

    void Update()
    {
           
    }
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.linearVelocity = -transform.right * speed;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Wall"))
        {
            Destroy(gameObject);
        }

        if (other.CompareTag("Enemy"))
        {
            other.gameObject.GetComponent<EnemyHealth>().Health--;
            Destroy(gameObject);
        }
    }
}