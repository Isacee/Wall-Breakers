using UnityEngine;

public class WallSpawner : MonoBehaviour
{
    public GameObject wallPrefab;
    public Transform player;

    [Header("Spawn Settings")]
    public float spawnRadius = 30f;      // maximum distance from player
    public float minSpawnRadius = 8f;    // minimum safe distance from player

    private float nextSpawnTime = 0f;

    void Update()
    {
        if (Time.time >= nextSpawnTime)
        {
            SpawnWall();

            float interval = (GameManager.Instance != null)
                ? GameManager.Instance.spawnInterval
                : 3f;

            nextSpawnTime = Time.time + interval;
        }
    }

    void SpawnWall()
    {
        if (player == null)
        {
            Debug.LogWarning("WallSpawner: No player assigned!");
            return;
        }

        // 🔁 Generate a random point that is outside the min radius
        Vector2 randomCircle = Random.insideUnitCircle.normalized * Random.Range(minSpawnRadius, spawnRadius);
        Vector3 spawnPos = new Vector3(randomCircle.x, 0f, randomCircle.y) + player.position;

        // ✅ Spawn the wall
        GameObject wall = Instantiate(wallPrefab, spawnPos, Quaternion.identity);

        Wall wallScript = wall.GetComponent<Wall>();
        if (wallScript != null)
        {
            wallScript.target = player;
        }
    }
}
