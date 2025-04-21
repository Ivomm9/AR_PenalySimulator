using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class Ball : MonoBehaviour
{
    public GameObject cam;
    private Vector2 startTouchPosition;
    private Vector2 endTouchPosition;
    private Rigidbody rb;
    private bool isDragging = false;
    private bool grabbedBall = false;
    private bool isGrounded = false;

    public float throwForce = 1f;
    public float distanceFromCamera = 2f; // Distancia adicional desde la cámara

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        GameManager.Instance.onGameRestart.AddListener(ResetBall);
    }

    void Update()
    {
        if (GameManager.Instance.gameStarted == false)
        {
            return;
        }

        if (rb.velocity.magnitude < 0.1f && grabbedBall == false && isDragging == true && isGrounded == true)
        {
            Debug.Log("Ball Slow");
            GameManager.Instance.Restart();
        }

        if (isDragging == false)
        {
            transform.position = cam.transform.position + cam.transform.forward * distanceFromCamera;
            rb.velocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;
        }

        if (GameManager.Instance.isGamePaused) 
        {
            return;
        }

        if (Input.GetMouseButtonDown(0))
        {
            grabbedBall = true;
            startTouchPosition = Input.mousePosition;
            isDragging = true;
        }

        if (Input.GetMouseButton(0))
        {
            Vector3 mousePosition = Input.mousePosition;
            mousePosition.z = distanceFromCamera; // Usar la distancia desde la cámara
            rb.velocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;
            Vector3 worldPosition = Camera.main.ScreenToWorldPoint(mousePosition);
            transform.position = worldPosition;
        }

        if (Input.GetMouseButtonUp(0))
        {
         
            endTouchPosition = Input.mousePosition;
            ThrowBall();
        }

        // Touch controls
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);

            if (touch.phase == UnityEngine.TouchPhase.Began)
            {
                startTouchPosition = touch.position;
            }

            if (touch.phase == UnityEngine.TouchPhase.Moved || touch.phase == UnityEngine.TouchPhase.Stationary)
            {
                Vector3 touchPosition = touch.position;
                touchPosition.z = distanceFromCamera; // Usar la distancia desde la cámara
                rb.velocity = Vector3.zero;
                rb.angularVelocity = Vector3.zero;
                Vector3 worldPosition = Camera.main.ScreenToWorldPoint(touchPosition);
                transform.position = worldPosition;
            }

            if (touch.phase == UnityEngine.TouchPhase.Ended)
            {
                endTouchPosition = touch.position;
                ThrowBall();
            }
        }

    }

    void ThrowBall()
    {
        float force = endTouchPosition.y - startTouchPosition.y;
        Debug.Log("Force: " + force);
        float distance = force;
        float adjustedThrowForce = throwForce * distance;

        GameManager.Instance.AddThrows(1);
        Vector3 throwDirection = (Camera.main.transform.forward).normalized;
        rb.AddForce((throwDirection * adjustedThrowForce) + new Vector3(0, adjustedThrowForce, 0), ForceMode.Impulse);
        grabbedBall = false;
    }

    void ResetBall()
    {
        isDragging = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Reseter"))
        {
            GameManager.Instance.Restart();
        }
        if (other.CompareTag("Goal") && !GameManager.Instance.isGamePaused)
        {
            GameManager.Instance.AddScore(1);
            GameManager.Instance.Restart();
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Floor"))
        {
            isGrounded = true;
        }

        if (collision.gameObject.CompareTag("Goalee") || collision.gameObject.CompareTag("Defense"))
        {
            GameManager.Instance.Restart();
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.CompareTag("Floor"))
        {
            isGrounded = false;
        }
    }

}
