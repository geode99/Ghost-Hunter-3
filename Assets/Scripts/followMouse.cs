using UnityEngine;
using UnityEngine.InputSystem;

public class followMouse : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        Vector3 MousePOS = Camera.main.ScreenToWorldPoint(Mouse.current.position.ReadValue());
        Vector2 Direction = MousePOS - transform.position;
        transform.right = Direction;
    }
}