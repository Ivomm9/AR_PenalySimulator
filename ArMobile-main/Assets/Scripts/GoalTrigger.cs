using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoalTrigger : MonoBehaviour
{
    [SerializeField] ParticleSystem goalParticleSystem;
    [SerializeField] AudioSource curveShotAudio;

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Ball"))
        {
            Ball ballScript = other.GetComponent<Ball>();

            if (ballScript != null)
            {
                if (ballScript.wasCurveShot)
                {
                    Debug.Log("¡Gol con curva!");
                    curveShotAudio.Play();

                }
                else
                {
                    Debug.Log("Gol normal");
                }
               
                goalParticleSystem.Play();
            }
        }
    }
}
