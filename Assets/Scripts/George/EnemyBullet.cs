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

        float angle = Vector3.Angle(transform.position, player.transform.position);
        Vector2 velocity = Quaternion.AngleAxis(angle, Vector3.up) * Vector3.right;
        rb.linearVelocity = velocity * speed;

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
