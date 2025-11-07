using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [Header("Asignar en Inspector")]
    public GameObject enemyPrefab;
    public Transform[] spawnPoints; // arrastra Spawn1, Spawn2, Spawn3 aquí

    [Header("Ajustes")]
    public float initialDelay = 0.5f;
    public float spawnInterval = 2.0f; // cada cuánto spawnea un enemigo
    public int maxAlive = 30; // máximo de enemigos activos

    List<GameObject> alive = new List<GameObject>();

    void Start()
    {
        if (enemyPrefab == null || spawnPoints == null || spawnPoints.Length == 0)
        {
            Debug.LogError("EnemySpawnerSimple: asigna enemyPrefab y al menos un spawnPoint.");
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
            // limpiar la lista de objetos destruidos/desactivados
            alive.RemoveAll(x => x == null || !x.activeInHierarchy);

            if (alive.Count < maxAlive)
            {
                int idx = Random.Range(0, spawnPoints.Length);
                Transform sp = spawnPoints[idx];

                GameObject go = Instantiate(enemyPrefab, sp.position, Quaternion.identity);
                alive.Add(go);
            }

            yield return new WaitForSeconds(spawnInterval);
        }
    }
}

