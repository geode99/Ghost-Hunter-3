using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class GridMovement : MonoBehaviour
{
    public float moveSpeed = 5f;
    [Tooltip("Select the layer used for walls and obstacles.")]
    public LayerMask obstacleLayer;

    private bool isMoving = false;
    private Vector2 origPos, targetPos;
    private InputAction moveAction;

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
    }

    void OnEnable() => moveAction.Enable();
    void OnDisable() => moveAction.Disable();

    void Update()
    {
        if (!isMoving)
        {
            Vector2 input = moveAction.ReadValue<Vector2>();

            if (input.x != 0) input.y = 0; // Prevent diagonal movement

            if (input.x != 0 || input.y != 0)
            {
                Vector3 direction = new Vector3(Mathf.RoundToInt(input.x), Mathf.RoundToInt(input.y), 0);

                // Calculate where the player wants to go
                Vector2 checkTarget = (Vector2)transform.position + (Vector2)direction;

                // Check if the target tile contains an obstacle before starting movement
                if (IsTileWalkable(checkTarget))
                {
                    StartCoroutine(MovePlayer(direction));
                }
            }
        }
    }

    bool IsTileWalkable(Vector2 targetPos)
    {
        // Cast a tiny circle at the target position to see if it hits the obstacle layer
        // A radius of 0.2f ensures it checks only the center area of the next tile
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
            transform.position = Vector3.Lerp(origPos, targetPos, elapsedTime / timeToMove);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        transform.position = targetPos;
        isMoving = false;
    }
}
