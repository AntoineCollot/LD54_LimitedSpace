using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemSpawner : MonoBehaviour
{
    [System.Serializable]
    public struct SpawnGroupData
    {
        public float spawnTime;
        public GameObject[] toSpawn;
    }
    public List<SpawnGroupData> spawnGroups;
    [SerializeField] GameObject apparitionSmokeFX;

    List<GameObject> itemsToSpawn = new List<GameObject>();

    const float MIN_DIST_TO_PLAYER = 3;

    // Update is called once per frame
    void Update()
    {
        CheckSpawn();
        SpawnItems();
    }

    void CheckSpawn()
    {
        for (int i = 0; i < spawnGroups.Count; i++)
        {
            if (Time.timeSinceLevelLoad >= spawnGroups[i].spawnTime)
            {
                SpawnGroup(i);
            }
        }
    }

    void SpawnGroup(int id)
    {
        foreach (GameObject obj in spawnGroups[id].toSpawn)
        {
            itemsToSpawn.Add(obj);
        }

        spawnGroups.RemoveAt(id);
    }

    void SpawnItems()
    {
        for (int i = itemsToSpawn.Count - 1; i >= 0; i--)
        {
            if (Vector2.Distance(PlayerState.Instance.CenterOfMass, itemsToSpawn[i].transform.position) >= MIN_DIST_TO_PLAYER)
            {
                itemsToSpawn[i].SetActive(true);

                GameObject fx = Instantiate(apparitionSmokeFX, transform);
                fx.transform.position = itemsToSpawn[i].transform.position;

                itemsToSpawn.RemoveAt(i);
            }
        }
    }
}
