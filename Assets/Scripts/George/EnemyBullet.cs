using UnityEngine;

public class EnemyBullet : MonoBehaviour
{
    public GameObject player;
    public Rigidbody2D rb;
    public float speed;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        player = FindAnyObjectByType<GridMovement>().gameObject;

        Vector3 heading = player.transform.position - transform.position;

        // 2. Normalize it to get a pure direction vector (length of 1)
        Vector3 direction = heading.normalized;
        rb.linearVelocity = direction * speed;

    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Wall")) {
            Destroy(gameObject);
        }
    }
}
