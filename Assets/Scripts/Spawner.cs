using UnityEngine;

public class WallSpawner : MonoBehaviour
{
    public GameObject wallPrefab;
    public Transform player; // turret base or pivot
    public float spawnRadius = 20f;
    public float spawnInterval = 3f;

    private float nextSpawnTime = 0f;

    void Update()
    {
        if (Time.time >= nextSpawnTime)
        {
            SpawnWall();
            nextSpawnTime = Time.time + spawnInterval;
        }
    }

    void SpawnWall()
    {
        Vector2 randomCircle = Random.insideUnitCircle.normalized * spawnRadius;
        Vector3 spawnPos = new Vector3(randomCircle.x, 0f, randomCircle.y);

        GameObject wall = Instantiate(wallPrefab, spawnPos, Quaternion.identity);

        Wall wallScript = wall.GetComponent<Wall>();
        if (wallScript != null)
        {
            wallScript.target = player; // ? assigns the turret
        }
    }
}
