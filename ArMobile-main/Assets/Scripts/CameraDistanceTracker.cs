using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraDistanceTracker : MonoBehaviour
{
    public Camera cam;
    [SerializeField] GameObject goal;
    [SerializeField] float distanceToGoal = 5f;
    [SerializeField] GameObject warning;

    private void Start()
    {
        cam = Camera.main;
    }

    // Update is called once per frame
    void Update()
    {
        if (cam != null && Vector3.Distance(cam.gameObject.transform.position, goal.transform.position) < distanceToGoal)
        {
            warning.SetActive(true);
            GameManager.Instance.isGamePaused = true;
        }
        else
        {
            warning.SetActive(false);
            GameManager.Instance.isGamePaused = false;
        }
    }
}
