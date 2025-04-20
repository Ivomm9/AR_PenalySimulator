using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DefenseJump : MonoBehaviour
{

    bool isJumping = false;

    bool movesLeftRight = false;

    Rigidbody rb;
    Vector3 initialPosition; 
    float oscillationSpeed = 2f; 
    float oscillationAmplitude = 1f;
    float oscillationTime = 0f; 
    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.isKinematic = true;
        GameManager.Instance.onGameRestart.AddListener(Dissapear);

        if (Random.Range(0, 2) == 0)
        {
            movesLeftRight = true;
            oscillationSpeed = Random.Range(1f, 3f);
            oscillationAmplitude = Random.Range(0.1f, 0.22f);
            initialPosition = transform.localPosition;
        }
        else
        {
            movesLeftRight = false;
        }
    }
    void Update()
    {
        if (Random.Range(0, 100) < 1 && !isJumping && rb.velocity.magnitude == 0 && !movesLeftRight)
        {
            Jump();
        }
        
        if (movesLeftRight)
        {
            OscillateLeftRight();
        }

    }

    void OscillateLeftRight()
    {
        oscillationTime += Time.deltaTime * oscillationSpeed;

        float offsetX = Mathf.Sin(oscillationTime) * oscillationAmplitude;

        transform.localPosition = new Vector3(initialPosition.x + offsetX, initialPosition.y, initialPosition.z);
    }

    void Jump()
    {
        rb.isKinematic = false;
        //isJumping = true;
        //Jump
        rb.AddForce(Vector3.up * 100 , ForceMode.Force);
    }

    void Dissapear() 
    {
        Destroy(gameObject);
    }
}
