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
            Ball ballScript = other.GetComponent<Ball>();

            if (ballScript != null)
            {
                if (ballScript.wasCurveShot)
                {
                    Debug.Log("¡Gol con curva!");
                    // Acá podés poner efectos o lógica diferente si querés
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
