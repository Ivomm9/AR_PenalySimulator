using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DefenseSpawner : MonoBehaviour
{
    [SerializeField] private Transform[] defensePositions;
    [SerializeField] private GameObject defensePrefab;
    void Start()
    {
        SpawnDefense();
        GameManager.Instance.onGameRestart.AddListener(SpawnDefense);
    }
    void SpawnDefense()
    {
        foreach (Transform position in defensePositions)
        {
            if (Random.Range(0, 2) == 0)
            {
                continue;
            }

            GameObject defense = Instantiate(defensePrefab, position.position, Quaternion.identity) as GameObject;
            defense.transform.SetParent(position);

            defense.transform.rotation = position.rotation * Quaternion.Euler(0, 90, 0);
        }
    }
}
