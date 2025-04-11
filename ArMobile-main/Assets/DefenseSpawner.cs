using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DefenseSpawner : MonoBehaviour
{
    [SerializeField] private Transform[] defensePositions;
    [SerializeField] private GameObject defensePrefab;
    void Start()
    {
        foreach (Transform position in defensePositions)
        {
            // Randomly decide if there needs to be a defense at this position or not

            if (Random.Range(0, 2) == 0)
            {
                continue;
            }

            GameObject defense = Instantiate(defensePrefab, position.position, Quaternion.identity) as GameObject;
            defense.transform.SetParent(position);
            //equal rotation to parent plus 90º
            defense.transform.rotation = position.rotation * Quaternion.Euler(0, 90, 0);
        }
    }

}
