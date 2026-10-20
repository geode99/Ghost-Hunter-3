using System.Collections;
using UnityEngine;

public class GridMovement : MonoBehaviour
{
    public float moveSpeed = 5f;
    public LayerMask obstacleLayer;
    [Header("Shooting")]
    public GameObject bulletPrefab;
    public Transform aimPivot;

    private bool isMoving;
    private Vector2 aimDir = Vector2.down;

    void Update()
    {
        // 1. 8-Directional Aiming Input (Mouse World Position conversion)
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector2 lookDir = ((Vector2)mousePos - (Vector2)transform.position).normalized;

        // Snap smooth mouse direction to 8-way vector steps via math
        float angle = Mathf.Atan2(lookDir.y, lookDir.x) * Mathf.Rad2Deg;
        float snappedAngle = Mathf.Floor(((angle < 0 ? angle + 360 : angle) + 22.5f) / 45f) * 45f;
        aimDir = new Vector2(Mathf.Cos(snappedAngle * Mathf.Deg2Rad), Mathf.Sin(snappedAngle * Mathf.Deg2Rad));

        if (aimPivot) aimPivot.rotation = Quaternion.Euler(0, 0, snappedAngle);

        // 2. 4-Directional Grid Movement
        if (!isMoving)
        {
            float x = Input.GetAxisRaw("Horizontal"), y = Input.GetAxisRaw("Vertical");
            Vector2 input = x != 0 ? new Vector2(x, 0) : new Vector2(0, y); // Enforce 4-way axis lock

            if (input != Vector2.zero && !Physics2D.OverlapCircle((Vector2)transform.position + input, 0.2f, obstacleLayer))
                StartCoroutine(MovePlayer(input));
        }

        // 3. Fire Weapon Command
        if (Input.GetKeyDown(KeyCode.Space)) Shoot();
    }

    IEnumerator MovePlayer(Vector2 dir)
    {
        isMoving = true;
        Vector3 start = transform.position, target = start + (Vector3)dir;
        float t = 0, duration = 1f / moveSpeed;

        while (t < duration)
        {
            transform.position = Vector3.Lerp(start, target, t / duration);
            t += Time.deltaTime;
            yield return null;
        }
        transform.position = target;
        isMoving = false;
    }

    void Shoot()
    {
        if (!bulletPrefab) return;
        GameObject b = Instantiate(bulletPrefab, transform.position + (Vector3)aimDir * 0.6f, Quaternion.identity);

        // Safer universal script check that won't throw errors regardless of class capitalization
        Component bScript = b.GetComponent("Bullet") ?? b.GetComponent("bullet");
        if (bScript != null)
        {
            var f = bScript.GetType().GetField("direction") ?? bScript.GetType().GetField("moveDir");
            if (f != null) f.SetValue(bScript, aimDir);
        }
    }
}
