using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenuActivation : MonoBehaviour
{
    public List<ItemSpawner.SpawnGroupData> spawnGroups;
    float startTime = 0;

    private void Start()
    {
        for (int i = 0; i < spawnGroups.Count; i++)
        {
            foreach (GameObject obj in spawnGroups[i].toSpawn)
            {
                obj.SetActive(false);
            }
        }

        startTime = Time.time;
    }

    private void Update()
    {
        for (int i = 0; i < spawnGroups.Count; i++)
        {
            if (Time.time - startTime >= spawnGroups[i].spawnTime)
            {
                foreach (GameObject obj in spawnGroups[i].toSpawn)
                {
                    obj.SetActive(true);
                }

                spawnGroups.RemoveAt(i);
            }
        }
    }
}
