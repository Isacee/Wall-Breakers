using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    [Header("Score System")]
    public int score = 0;

    [Header("Difficulty Settings")]
    public int difficultyLevel = 1;
    public int scorePerLevel = 100;       // every 100 points = harder
    public float spawnInterval = 3f;      // starting spawn rate
    public float minSpawnInterval = 0.8f; // fastest possible spawn
    public float difficultyStep = 0.3f;   // how much spawn interval decreases per level
    public float wallHealthMultiplier = 1.2f; // walls get 20% stronger per level

    void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }

    public void AddScore(int points)
    {
        score += points;
        Debug.Log($"Score: {score}");

        // ✅ Check if we should increase difficulty
        if (score >= difficultyLevel * scorePerLevel)
        {
            IncreaseDifficulty();
        }
    }

    private void IncreaseDifficulty()
    {
        difficultyLevel++;
        spawnInterval = Mathf.Max(minSpawnInterval, spawnInterval - difficultyStep);

        Debug.Log($"⬆ Difficulty increased to {difficultyLevel}! New spawn rate: {spawnInterval}s, Wall health x{wallHealthMultiplier:F1}");
    }

    // helper for wall scaling
    public int GetScaledWallHealth(int baseHealth)
    {
        return Mathf.RoundToInt(baseHealth * Mathf.Pow(wallHealthMultiplier, difficultyLevel - 1));
    }

    public float GetDifficultyMultiplier()
    {
        return Mathf.Pow(wallHealthMultiplier, difficultyLevel - 1);
    }

}
