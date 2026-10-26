using UnityEngine;

public class GridMov : MonoBehaviour
{
    [Header("Movement & Grid")]
    public float moveSpeed = 5f;
    public float gridSize = 1f; // Adjust if your grid tiles are larger/smaller than 1 unit
    [Tooltip("Set this to your Wall/Obstacle layer. If left on 'Nothing', it will skip wall checks.")]
    public LayerMask obstacleLayer;

    [Header("Shooting")]
    public GameObject bulletPrefab;
    public Transform aimPivot;
    public Transform firePoint; // Optional: Where the bullet spawns (defaults to transform if null)

    [Header("2-Frame Animation")]
    public SpriteRenderer spriteRenderer;
    public Sprite idleSprite;
    public Sprite walkSprite;
    public float animationFrameRate = 0.15f;

    private bool isMoving;
    private Vector2 targetPosition;
    private float animationTimer;
    private bool showWalkFrame;
    private Camera mainCamera;

    void Awake()
    {
        if (spriteRenderer == null) spriteRenderer = GetComponent<SpriteRenderer>();
        mainCamera = Camera.main;
        targetPosition = transform.position;
    }

    void Update()
    {
        // Safe camera fallback
        if (mainCamera == null)
        {
            mainCamera = Camera.main;
            if (mainCamera == null) return;
        }

        HandleAimingAndFlipping();
        HandleGridMovement();
        HandleShooting();
        HandleAnimation();
    }

    private void HandleAimingAndFlipping()
    {
        Vector3 mousePos = mainCamera.ScreenToWorldPoint(Input.mousePosition);
        Vector2 lookDir = ((Vector2)mousePos - (Vector2)transform.position).normalized;

        // Flip character transform scale safely
        Vector3 currentScale = transform.localScale;
        if (mousePos.x < transform.position.x - 0.01f)
        {
            transform.localScale = new Vector3(-Mathf.Abs(currentScale.x), currentScale.y, currentScale.z);
        }
        else if (mousePos.x > transform.position.x + 0.01f)
        {
            transform.localScale = new Vector3(Mathf.Abs(currentScale.x), currentScale.y, currentScale.z);
        }

        // 8-Way Angle Snapping
        float angle = Mathf.Atan2(lookDir.y, lookDir.x) * Mathf.Rad2Deg;
        float snappedAngle = Mathf.Floor(((angle < 0 ? angle + 360 : angle) + 22.5f) / 45f) * 45f;

        // Correct for inverted local space when flipped left
        if (aimPivot != null)
        {
            if (transform.localScale.x < 0)
                aimPivot.localRotation = Quaternion.Euler(0, 0, -snappedAngle + 180f);
            else
                aimPivot.localRotation = Quaternion.Euler(0, 0, snappedAngle);
        }
    }

    private void HandleGridMovement()
    {
        // Smoothly slide toward the grid target position
        transform.position = Vector3.MoveTowards(transform.position, targetPosition, moveSpeed * Time.deltaTime);

        // Check if we have arrived at our grid tile
        if (Vector3.Distance(transform.position, targetPosition) < 0.01f)
        {
            isMoving = false;

            // Read standard Unity inputs (Works on every PC automatically)
            float h = Input.GetAxisRaw("Horizontal");
            float v = Input.GetAxisRaw("Vertical");

            if (h != 0 || v != 0)
            {
                // Prioritize horizontal movement over vertical for clean grid snapping, or allow diagonals
                Vector2 moveDir = new Vector2(h, v).normalized;

                // Snap to strict 4-way or 8-way grid steps
                if (Mathf.Abs(h) >= Mathf.Abs(v)) moveDir = new Vector2(Mathf.Sign(h), 0);
                else moveDir = new Vector2(0, Mathf.Sign(v));

                Vector2 potentialTarget = (Vector2)transform.position + (moveDir * gridSize);

                // Collision Detection: Only blocks if obstacleLayer is specifically set up
                if (obstacleLayer != 0)
                {
                    RaycastHit2D hit = Physics2D.Raycast(transform.position, moveDir, gridSize, obstacleLayer);
                    if (hit.collider == null)
                    {
                        targetPosition = potentialTarget;
                        isMoving = true;
                    }
                }
                else
                {
                    targetPosition = potentialTarget;
                    isMoving = true;
                }
            }
        }
    }

    private void HandleShooting()
    {
        // Reads standard Left Mouse Click or Ctrl key
        if (Input.GetButtonDown("Fire1") && bulletPrefab != null)
        {
            Transform spawnPoint = firePoint != null ? firePoint : transform;

            // Calculate bullet trajectory based on where the pivot is pointing
            Quaternion bulletRotation = aimPivot != null ? aimPivot.rotation : transform.rotation;

            Instantiate(bulletPrefab, spawnPoint.position, bulletRotation);
        }
    }

    private void HandleAnimation()
    {
        if (spriteRenderer == null || idleSprite == null || walkSprite == null) return;

        if (isMoving)
        {
            animationTimer += Time.deltaTime;
            if (animationTimer >= animationFrameRate)
            {
                animationTimer = 0f;
                showWalkFrame = !showWalkFrame;
                spriteRenderer.sprite = showWalkFrame ? walkSprite : idleSprite;
            }
        }
        else
        {
            animationTimer = 0f;
            showWalkFrame = false;
            spriteRenderer.sprite = idleSprite;
        }
    }
}



