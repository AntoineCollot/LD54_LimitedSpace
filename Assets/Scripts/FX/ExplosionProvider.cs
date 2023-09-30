using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplosionProvider : MonoBehaviour
{
    [SerializeField] GameObject singleExplosionPrefab;
    [SerializeField] GameObject enemyDeathPrefab;

    public static ExplosionProvider Instance;

    void Awake()
    {
        Instance = this;
    }

    public void SpawnSingleExplosion(Vector3 pos)
    {
        GameObject explo = Instantiate(singleExplosionPrefab, transform);
        explo.transform.position = pos;
    }

    public void SpawnEnemyDeath(Vector3 pos)
    {
        GameObject explo = Instantiate(enemyDeathPrefab, transform);
        explo.transform.position = pos;
    }
}
