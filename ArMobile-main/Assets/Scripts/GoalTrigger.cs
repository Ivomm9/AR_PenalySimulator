using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoalTrigger : MonoBehaviour
{
    [SerializeField] ParticleSystem goalParticleSystem;

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Ball"))
        {
            goalParticleSystem.Play();
        }
    }
}