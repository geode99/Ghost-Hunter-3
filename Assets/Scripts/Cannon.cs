using UnityEngine;

public class CannonShoot : MonoBehaviour
{
    public GameObject bulletPrefab;
    public Transform firePoint;
    public float bulletSpeed = 10f;

    [Header("Automatic Shooting Settings")]
    public float fireRate = 2f; // Time in seconds between shots
    private float nextFireTime = 0f;
    public bool NotPlacing = true;

    public Sprite forward;
    public Sprite backward;
    public Sprite left;
    public Sprite right;

    void Update()
    {
        // Automatically shoots when the timer reaches 0
        if (Time.time >= nextFireTime && NotPlacing)
        {
            Shoot();
            nextFireTime = Time.time + fireRate; // Reset the timer
        }
        if(gameObject.transform.rotation.eulerAngles.z == 0)
        {
            gameObject.transform.parent.GetComponent<SpriteRenderer>().sprite = forward;
        }
        else if (gameObject.transform.rotation.eulerAngles.z == 90)
        {
            gameObject.transform.parent.GetComponent<SpriteRenderer>().sprite = right;
        }
        else if(gameObject.transform.rotation.eulerAngles.z == 180)
        {
            gameObject.transform.parent.GetComponent<SpriteRenderer>().sprite = backward;
        }
        else if(gameObject.transform.rotation.eulerAngles.z == 270)
        {
            gameObject.transform.parent.GetComponent<SpriteRenderer>().sprite = left;
        }
    }

    void Shoot()
    {
        if (bulletPrefab == null || firePoint == null) return;

        GameObject bullet = Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);

        Rigidbody2D rb = bullet.GetComponent<Rigidbody2D>();
        if (rb != null)
        {
            rb.linearVelocity = firePoint.up * -bulletSpeed;
        }
    }
}
