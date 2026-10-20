using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class GridMovement : MonoBehaviour
{
    public float moveSpeed = 5f;
    [Tooltip("Select the layer used for walls and obstacles.")]
    public LayerMask obstacleLayer;

    [Header("Shooting")]
    public GameObject bulletPrefab;

    private bool isMoving = false;
    private Vector2 origPos, targetPos;
    private InputAction moveAction;
    private InputAction shootAction;

    private Vector2 facingDirection = Vector2.down;

    void Awake()
    {
        moveAction = new InputAction("Move", binding: "<Gamepad>/dpad");
        moveAction.AddCompositeBinding("2DVector")
            .With("Up", "<Keyboard>/w")
            .With("Up", "<Keyboard>/upArrow")
            .With("Down", "<Keyboard>/s")
            .With("Down", "<Keyboard>/downArrow")
            .With("Left", "<Keyboard>/a")
            .With("Left", "<Keyboard>/leftArrow")
            .With("Right", "<Keyboard>/d")
            .With("Right", "<Keyboard>/rightArrow");

        shootAction = new InputAction("Shoot", binding: "<Keyboard>/space");
        shootAction.AddBinding("<Gamepad>/buttonSouth");
    }

    void OnEnable()
    {
        moveAction.Enable();
        shootAction.Enable();
    }

    void OnDisable()
    {
        moveAction.Disable();
        shootAction.Disable();
    }

    void Update()
    {
        if (!isMoving)
        {
            Vector2 input = moveAction.ReadValue<Vector2>();

            if (input.x != 0)
                input.y = 0; // Prevent diagonal movement

            if (input.x != 0 || input.y != 0)
            {
                Vector3 direction = new Vector3(
                    Mathf.RoundToInt(input.x),
                    Mathf.RoundToInt(input.y),
                    0);

                facingDirection = direction;

                Vector2 checkTarget = (Vector2)transform.position + (Vector2)direction;

                if (IsTileWalkable(checkTarget))
                {
                    StartCoroutine(MovePlayer(direction));
                }
            }
        }

        if (shootAction.WasPressedThisFrame())
        {
            Shoot();
        }
    }

    bool IsTileWalkable(Vector2 targetPos)
    {
        return !Physics2D.OverlapCircle(targetPos, 0.2f, obstacleLayer);
    }

    IEnumerator MovePlayer(Vector3 direction)
    {
        isMoving = true;

        origPos = transform.position;
        targetPos = (Vector2)transform.position + (Vector2)direction;

        float elapsedTime = 0;
        float timeToMove = 1f / moveSpeed;

        while (elapsedTime < timeToMove)
        {
            transform.position = Vector3.Lerp(
                origPos,
                targetPos,
                elapsedTime / timeToMove);

            elapsedTime += Time.deltaTime;
            yield return null;
        }

        transform.position = targetPos;
        isMoving = false;
    }

    void Shoot()
    {
        if (bulletPrefab == null)
            return;

        Vector3 spawnPos = transform.position + (Vector3)facingDirection * 0.6f;

        GameObject bullet = Instantiate(
            bulletPrefab,
            spawnPos,
            Quaternion.identity);

        Bullet bulletScript = bullet.GetComponent<Bullet>();

        if (bulletScript != null)
        {
            bulletScript.direction = facingDirection;
        }
    }
}