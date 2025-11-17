using UnityEngine;

public class CoinSpawnerSimple : MonoBehaviour
{
    public GameObject coinPrefab;
    public int amount = 10;         // Cuántas monedas quieres generar
    public Vector2 areaSize = new Vector2(10, 10); // Tamaño del área donde aparecerán

    void Start()
    {
        SpawnCoins();
    }

    void SpawnCoins()
    {
        for (int i = 0; i < amount; i++)
        {
            Vector2 randomPos = new Vector2(
                Random.Range(transform.position.x - areaSize.x / 2, transform.position.x + areaSize.x / 2),
                Random.Range(transform.position.y - areaSize.y / 2, transform.position.y + areaSize.y / 2)
            );

            Instantiate(coinPrefab, randomPos, Quaternion.identity);
        }
    }

    // Solo para ver el área en la escena
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireCube(transform.position, areaSize);
    }
}

