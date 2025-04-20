using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallDistanceTracker : MonoBehaviour
{
    [SerializeField] GoaleeScript goaleeScript;
    GameObject ball;


    void Start()
    {
        ball = GameObject.FindGameObjectWithTag("Ball");
    }

    // Update is called once per frame
    void Update()
    {
        if (ball != null && Vector3.Distance(ball.transform.position, transform.position) < 0.7f)
        {
            goaleeScript.Jump(Random.Range(1, 4));
        }
    }
}
