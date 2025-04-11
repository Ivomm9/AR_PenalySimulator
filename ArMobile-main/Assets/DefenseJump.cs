using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DefenseJump : MonoBehaviour
{

    bool isJumping = false;

    Rigidbody rb;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.isKinematic = true;
    }
    void Update()
    {
        if (Random.Range(0, 100) < 1 && !isJumping && rb.velocity.magnitude == 0)
        {
            Jump();
        }

       //if (isJumping)
       //{
       //
       //    if (rb.velocity.y == 0)
       //    {
       //        isJumping = false;
       //        rb.isKinematic = true;
       //        Debug.Log("Jumping stopped");
       //    }
       //}
    }

    void Jump()
    {
        rb.isKinematic = false;
        //isJumping = true;
        //Jump
        rb.AddForce(Vector3.up * 100 , ForceMode.Force);
    }
}
