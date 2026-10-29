using UnityEngine;
using UnityEngine.InputSystem;

public class FollowAim : MonoBehaviour
{
    [SerializeField] private float stickDeadzone = 0.4f;
    [SerializeField] private float mouseMoveThreshold = 2f;

    private Camera cam;
    private bool usingGamepad = false;

    void Awake()
    {
        cam = Camera.main;
    }

    void Update()
    {
        // RIGHT STICK INPUT
        if (Gamepad.current != null)
        {
            Vector2 stick = Gamepad.current.rightStick.ReadValue();

            if (stick.magnitude > stickDeadzone)
            {
                usingGamepad = true;
                transform.up = stick.normalized;
                return;
            }
        }

        // Only switch back if mouse ACTUALLY moved
        if (Mouse.current != null)
        {
            Vector2 mouseDelta = Mouse.current.delta.ReadValue();

            if (mouseDelta.magnitude > mouseMoveThreshold)
            {
                usingGamepad = false;
            }

            if (!usingGamepad)
            {
                Vector3 mousePos = cam.ScreenToWorldPoint(Mouse.current.position.ReadValue());
                mousePos.z = 0;

                Vector2 dir = mousePos - transform.position;

                if (dir.sqrMagnitude > 0.001f)
                    transform.up = dir.normalized;
            }
        }
    }
}
