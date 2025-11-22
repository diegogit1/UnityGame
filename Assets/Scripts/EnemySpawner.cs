using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [Header("Asignar en Inspector")]
    public GameObject enemyPrefab;
    public Transform[] spawnPoints;

    [Header("Ajustes")]
    public float initialDelay = 0.5f;
    public float spawnInterval = 2.0f; // cada cuánto spawnea un enemigo
    public int maxAlive = 30; // máximo de enemigos activos

    [Header("Dificultad")]
    public int enemiesPerSpawnNormal = 1;
    public int enemiesPerSpawnHard = 2;
    private int enemiesPerSpawn = 1;

    List<GameObject> alive = new List<GameObject>();

    void Start()
    {
        if (enemyPrefab == null || spawnPoints == null || spawnPoints.Length == 0)
        {
            Debug.LogError("EnemySpawner: asigna enemyPrefab y al menos un spawnPoint.");
            enabled = false;
            return;
        }

        StartCoroutine(SpawnLoop());
    }

    IEnumerator SpawnLoop()
    {
        yield return new WaitForSeconds(initialDelay);

        while (true)
        {
            alive.RemoveAll(x => x == null || !x.activeInHierarchy);

            if (alive.Count < maxAlive)
            {
                for (int i = 0; i < enemiesPerSpawn; i++)
                {
                    int idx = Random.Range(0, spawnPoints.Length);
                    Transform sp = spawnPoints[idx];
                    GameObject go = Instantiate(enemyPrefab, sp.position, Quaternion.identity);
                    alive.Add(go);
                }
            }

            yield return new WaitForSeconds(spawnInterval);
        }
    }

    public void SetDifficulty(bool hard)
    {
        if (hard)
        {
            spawnInterval = Mathf.Max(0.5f, spawnInterval / 2f); // más rápido
            maxAlive *= 2; // más enemigos activos
        }
        else
        {
            spawnInterval = 2f; // valores normales
            maxAlive = 30;
        }
    }

}


