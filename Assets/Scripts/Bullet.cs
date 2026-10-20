using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float speed = 10f;
    public Vector2 direction;

    void Update()
    {
        transform.position += (Vector3)(direction * speed * Time.deltaTime);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Wall"))
        {
            Destroy(gameObject);
        }

        if (other.CompareTag("Enemy"))
        {
            Destroy(other.gameObject);
            Destroy(gameObject);
        }
    }
}