using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Ball : MonoBehaviour
{
    private Vector2 startTouchPosition;
    private Vector2 endTouchPosition;
    private Rigidbody rb;
    private bool isDragging = false;

    public float throwForce = 1f;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    void Update()
    {
        if (Touchscreen.current.primaryTouch.press.isPressed || Mouse.current.leftButton.isPressed)
        {
            if (!isDragging)
            {
                // Start touch or mouse click
                startTouchPosition = Touchscreen.current.primaryTouch.press.isPressed
                    ? Touchscreen.current.primaryTouch.position.ReadValue()
                    : Mouse.current.position.ReadValue();
                isDragging = true;
            }
            else
            {
                // Update touch or mouse drag
                endTouchPosition = Touchscreen.current.primaryTouch.press.isPressed
                    ? Touchscreen.current.primaryTouch.position.ReadValue()
                    : Mouse.current.position.ReadValue();
                gameObject.transform.position = Camera.main.ScreenToWorldPoint(new Vector3(endTouchPosition.x, endTouchPosition.y, 1));
                rb.velocity = Vector3.zero;
            }
        }
        else if (isDragging)
        {
            // End touch or mouse release
            isDragging = false;
            ThrowBall();
        }
    }

    void ThrowBall()
    {
        Vector2 direction = endTouchPosition - startTouchPosition;
        Vector3 throwDirection = new Vector3(direction.x, 1, 1).normalized;
        rb.AddForce(throwDirection * throwForce, ForceMode.Impulse);
    }
}