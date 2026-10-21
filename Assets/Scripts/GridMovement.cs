using System.Collections;
using UnityEngine;

public class GridMovement : MonoBehaviour
{
    public float moveSpeed = 5f;
    public LayerMask obstacleLayer;

    [Header("Shooting")]
    public GameObject bulletPrefab;
    public Transform aimPivot;

    [Header("2-Frame Animation & Flipping")]
    public SpriteRenderer spriteRenderer; // Drag your SpriteRenderer here
    public Sprite idleSprite;             // Drag your Idle frame here
    public Sprite walkSprite;             // Drag your Walk frame here
    public float animationFrameRate = 0.15f; // Time in seconds between frame swaps

    private bool isMoving;
    private Vector2 aimDir = Vector2.down;
    private float animationTimer;
    private bool showWalkFrame;

    void Update()
    {
        // 1. Mouse Aiming & Horizontal Sprite Flipping
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector2 lookDir = ((Vector2)mousePos - (Vector2)transform.position).normalized;

        // Flip sprite based on mouse X position relative to the player
        if (spriteRenderer != null)
        {
            if (mousePos.x < transform.position.x)
                spriteRenderer.flipX = true;  // Face Left
            else if (mousePos.x > transform.position.x)
                spriteRenderer.flipX = false; // Face Right
        }

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

        // 3. Simple Script-Based Animation State Machine
        HandleAnimation();

        // 4. Fire Weapon Command
        if (Input.GetKeyDown(KeyCode.Space)) Shoot();
    }

    void HandleAnimation()
    {
        if (spriteRenderer == null || idleSprite == null || walkSprite == null) return;

        if (isMoving)
        {
            // Advance the timer while moving
            animationTimer += Time.deltaTime;
            if (animationTimer >= animationFrameRate)
            {
                animationTimer = 0f;
                showWalkFrame = !showWalkFrame; // Alternate back and forth
            }

            // Assign the correct frame based on our boolean toggle
            spriteRenderer.sprite = showWalkFrame ? walkSprite : idleSprite;
        }
        else
        {
            // Reset to standard idle when stationary
            spriteRenderer.sprite = idleSprite;
            animationTimer = 0f;
            showWalkFrame = false;
        }
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
