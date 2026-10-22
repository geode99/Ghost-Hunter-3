using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class GridMovement : MonoBehaviour
{
    [Header("Movement Settings")]
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private LayerMask obstacleLayer;
    [Header("Input System Setup")]
    [SerializeField] private InputActionReference moveAction;
    private bool isMoving;
    private void OnEnable()
    {
        moveAction.action.Enable();

        moveAction.action.performed += ProcessGridInput;
    }
    private void OnDisable()
    {
        moveAction.action.Disable();
        moveAction.action.performed -= ProcessGridInput;
    }
    public void ProcessGridInput(InputAction.CallbackContext context)
    {
        if (isMoving) return;
        StartCoroutine(ContinuousMovementLoop());
    }
    private IEnumerator ContinuousMovementLoop()
    {
        isMoving = true;
        while (moveAction.action.IsPressed())
        {
            Vector2 inputVector = moveAction.action.ReadValue<Vector2>();
            float horizontal = Mathf.Round(inputVector.x);
            float vertical = Mathf.Round(inputVector.y);
            if (horizontal != 0) vertical = 0;
            Vector2 direction = new Vector2(horizontal, vertical);
            if (direction != Vector2.zero)
            {
                Vector2 origPos = transform.position;
                Vector2 targetPos = origPos + direction;
                if (!Physics2D.Raycast(origPos, direction, 1f, obstacleLayer))
                {
                    float elapsed = 0f;
                    while (elapsed < 1f)
                    {
                        transform.position = Vector3.MoveTowards(origPos, targetPos, elapsed);
                        elapsed += Time.deltaTime * moveSpeed;
                        yield return null;
                    }
                    transform.position = targetPos;
                }
                else
                {
                    yield return null;
                }
            }
            else
            {
                yield return null;
            }
        }

        isMoving = false;
    }
}