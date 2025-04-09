using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Ball : MonoBehaviour
{
    public GameObject cam;
    private Vector2 startTouchPosition;
    private Vector2 endTouchPosition;
    private Rigidbody rb;
    private bool isDragging = false;

    public float throwForce = 1f;
    public float distanceFromCamera = 2f; // Distancia adicional desde la cámara



    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    void Update()
    {
        if (Input.GetMouseButton(0)) 
        {

            Vector3 mousePosition = Input.mousePosition;
            mousePosition.z = distanceFromCamera; // Usar la distancia desde la cámara
            rb.velocity = Vector3.zero;
            Vector3 worldPosition = Camera.main.ScreenToWorldPoint(mousePosition);
            transform.position = worldPosition;

        }
        else if (isDragging)
        {
            // Additional logic for when dragging ends
        }
    }

    void ThrowBall()
    {
        Vector2 direction = endTouchPosition - startTouchPosition;
        Vector3 throwDirection = new Vector3(direction.x, 1, 1).normalized;
        rb.AddForce(throwDirection * throwForce, ForceMode.Impulse);
    }
}
