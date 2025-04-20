using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoaleeScript : MonoBehaviour
{
    [Header("Jump Settings")]
    [SerializeField] private float jumpForce = 3f;
    [SerializeField] private float horizontalJumpForce = 2f;

    [SerializeField] GameObject mesh;

    private Rigidbody rb;
    private bool isJumping = false;
    private Vector3 initialPosition;
    private Quaternion initialRotation;

    private void Start()
    {
        GameManager.Instance.onGameRestart.AddListener(ResetPosition);
        rb = GetComponent<Rigidbody>();
        if (rb == null)
        {
            rb = gameObject.AddComponent<Rigidbody>();
        }

        initialPosition = transform.localPosition;
        initialRotation = mesh.transform.localRotation;

    }

    public void Jump(int jumpDirection)
    {
        if (isJumping)
            return;

        isJumping = true;

        rb.isKinematic = false;

        Vector3 jumpVector = Vector3.zero;

        switch (jumpDirection)
        {
            case 1: // Izquierda
                jumpVector = Vector3.up * jumpForce + transform.right* horizontalJumpForce;
                mesh.transform.localRotation = Quaternion.Euler(0, 0, -90);
                break;
            case 2: // Arriba
                jumpVector = Vector3.up * jumpForce;
                break;
            case 3: // Derecha
                jumpVector = Vector3.up * jumpForce + -transform.right * horizontalJumpForce;
                mesh.transform.localRotation = Quaternion.Euler(0, 0, 90);
                break;
            default:
                jumpVector = Vector3.up * jumpForce;
                break;
        }

        rb.velocity = Vector3.zero;
        rb.AddForce(jumpVector, ForceMode.Impulse);
    }

    public void ResetPosition()
    {
        rb.velocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;
        transform.localPosition = initialPosition;
        mesh.transform.localRotation = initialRotation;
        isJumping = false;
    }
}
