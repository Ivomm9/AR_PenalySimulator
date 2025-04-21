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
    [HideInInspector]
    public bool wasCurveShot = false;

    public float throwForce = 1f;
    public float distanceFromCamera = 2f; 


    private float spinDirection = 0f;
    private List<Vector2> dragPositions = new List<Vector2>();
    public float magnusFactor = 0.01f; 

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        GameManager.Instance.onGameRestart.AddListener(ResetBall);
    }

    void Update()
    {
        if (GameManager.Instance.gameStarted == false)
            return;

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
            return;

        //if (Input.GetMouseButtonDown(0))
        //{
        //    grabbedBall = true;
        //    startTouchPosition = Input.mousePosition;
        //    isDragging = true;
        //    dragPositions.Clear();
        //    dragPositions.Add(Input.mousePosition);
        //}
        //
        //if (Input.GetMouseButton(0))
        //{
        //    Vector3 mousePosition = Input.mousePosition;
        //    mousePosition.z = distanceFromCamera; 
        //    rb.velocity = Vector3.zero;
        //    rb.angularVelocity = Vector3.zero;
        //    Vector3 worldPosition = Camera.main.ScreenToWorldPoint(mousePosition);
        //    transform.position = worldPosition;
        //
        //    dragPositions.Add(Input.mousePosition);
        //}
        //
        //if (Input.GetMouseButtonUp(0))
        //{
        //    endTouchPosition = Input.mousePosition;
        //    CalculateSpin();  
        //    ThrowBall();      
        //}

        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);

            if (touch.phase == UnityEngine.TouchPhase.Began)
            {
                startTouchPosition = touch.position;
                isDragging = true;
                dragPositions.Clear();
                dragPositions.Add(touch.position);
            }

            if (touch.phase == UnityEngine.TouchPhase.Moved || touch.phase == UnityEngine.TouchPhase.Stationary)
            {
                Vector3 touchPosition = touch.position;
                touchPosition.z = distanceFromCamera; // Mantiene la distancia desde la cámara
                rb.velocity = Vector3.zero;
                rb.angularVelocity = Vector3.zero;
                Vector3 worldPosition = Camera.main.ScreenToWorldPoint(touchPosition);
                transform.position = worldPosition;

    
                dragPositions.Add(touch.position);
            }

            if (touch.phase == UnityEngine.TouchPhase.Ended)
            {
                endTouchPosition = touch.position;
                CalculateSpin();
                ThrowBall();
            }
        }
    }

    void FixedUpdate()
    {
        
        if (rb.velocity.magnitude > 0.1f && rb.angularVelocity.magnitude > 0.1f)
        {
            ApplyMagnusEffect();
        }
    }


    void ApplyMagnusEffect()
    {
        Vector3 spin = rb.angularVelocity;
        Vector3 magnusForce = Vector3.Cross(spin, rb.velocity) * magnusFactor;
        rb.AddForce(magnusForce, ForceMode.Force);
    }

 
    void CalculateSpin()
    {
        if (dragPositions.Count < 5)
        {
            spinDirection = 0f;
            return;
        }

        float curvature = 0f;
        for (int i = 2; i < dragPositions.Count; i++)
        {
            Vector2 a = dragPositions[i - 2];
            Vector2 b = dragPositions[i - 1];
            Vector2 c = dragPositions[i];

            Vector2 ab = (b - a).normalized;
            Vector2 bc = (c - b).normalized;

            float angle = Vector2.SignedAngle(ab, bc);
            curvature += angle * 1.5f;
        }

  
        if (Mathf.Abs(curvature) > 600)
        {
            spinDirection = Mathf.Sign(curvature);
            wasCurveShot = true;
        }
        else
        {
            spinDirection = 0f;
            wasCurveShot = false;
        }
        dragPositions.Clear();
    }


    void ThrowBall()
    {
        float force = endTouchPosition.y - startTouchPosition.y;
        float adjustedThrowForce = throwForce * force;

        GameManager.Instance.AddThrows(1);

        Vector3 forward = Camera.main.transform.forward.normalized;
        Vector3 totalForce = (forward * adjustedThrowForce) + new Vector3(0, adjustedThrowForce, 0);

        rb.AddForce(totalForce, ForceMode.Impulse);


        if (spinDirection != 0f)
        {
            rb.angularVelocity = Vector3.up * spinDirection * 20f; 
        }

        grabbedBall = false;
        spinDirection = 0f;
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
