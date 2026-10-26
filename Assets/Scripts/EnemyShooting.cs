using UnityEngine;

public class EnemyShooting : MonoBehaviour
{
    public float cooldown;
    public GameObject bullet;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (cooldown < 0) {
            Instantiate(bullet, transform.position, Quaternion.identity);
            cooldown = 2;
        }
        cooldown -= Time.deltaTime;
    }
}
