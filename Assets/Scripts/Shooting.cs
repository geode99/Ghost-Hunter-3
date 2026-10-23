using UnityEngine;
using UnityEngine.InputSystem;

public class Shooting : MonoBehaviour
{
    public GameObject firePoint;
    public GameObject bullet;

    public void Shoot(InputAction.CallbackContext ctx)
    {
        if (ctx.started)
        {


            GameObject bulletInstance = Instantiate(bullet, firePoint.transform.position, firePoint.transform.rotation);
        }
    }
}
