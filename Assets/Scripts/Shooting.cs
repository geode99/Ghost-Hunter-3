using UnityEngine;
using UnityEngine.InputSystem;

public class Shooting : MonoBehaviour
{

    public float speed;
    public float recoil;
    public GameObject bullet;
    public float lifetime;

    public void Shoot(InputAction.CallbackContext ctx)
    {
        if (ctx.started)
        {


            GameObject bulletInstance = Instantiate(bullet, transform.position, transform.rotation);

            bulletInstance.GetComponent<Rigidbody2D>().linearVelocity = -transform.up * speed;
            transform.parent.parent.GetComponent<Rigidbody2D>().linearVelocity = transform.up * recoil;
            Destroy(bulletInstance, lifetime);
        }
    }
}
