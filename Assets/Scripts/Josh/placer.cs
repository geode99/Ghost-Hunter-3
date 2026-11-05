using UnityEngine;
using UnityEngine.InputSystem;

public class placer : MonoBehaviour
{
    public Vector2Int Direction { get; private set; } = Vector2Int.right;

    private Camera cam;

    void Start()
    {
        cam = Camera.main;
    }

    void Update()
    {
        if (cam == null || Mouse.current == null) return;

        Vector3 mouseWorld = cam.ScreenToWorldPoint(Mouse.current.position.ReadValue());
        Vector2 delta = mouseWorld - transform.position;

        if (delta.sqrMagnitude < 0.0001f) return;

        if (Mathf.Abs(delta.x) >= Mathf.Abs(delta.y))
            Direction = new Vector2Int(delta.x > 0 ? 1 : -1, 0);
        else
            Direction = new Vector2Int(0, delta.y > 0 ? 1 : -1);
    }
}