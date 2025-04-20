using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class GoalSpawn : MonoBehaviour
{
    Camera cam;
    Button button;

    void Start()
    {
        cam = Camera.main;
        if (cam == null)
        {
            Debug.LogError("No se encontró la cámara principal.");
        }
        GameManager.Instance.gameStarted = true;

        button = GameObject.Find("AdjustPosButton").GetComponent<Button>();
        button.onClick.AddListener(AdjustPos);

        AdjustVerticalPos(cam.gameObject.gameObject.transform.position.y - 1f);
    }

    public void AdjustVerticalPos(float y)
    {
        Vector3 newPos = transform.position;
        newPos.y = y;
        transform.position = newPos;
    }

    public void AdjustPos()
    {
        if (cam == null)
        {
            Debug.LogError("Didnt find main camera.");
            return;
        }

        Vector3 cameraForward = cam.transform.forward;
        Vector3 newPosition = cam.transform.position + cameraForward * 3;

        transform.position = newPosition;

        Vector3 lookAtPosition = cam.transform.position;
        lookAtPosition.y = transform.position.y;
        transform.LookAt(lookAtPosition);

    }
}
