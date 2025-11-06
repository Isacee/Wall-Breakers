using UnityEngine;

public class Wall : MonoBehaviour
{
    public Transform target;
    public float moveSpeed = 2f;
    public int baseHealth = 100;
    public int health;
    public int scoreValue = 1;

    [Header("Rising Settings")]
    public float riseSpeed = 2f;
    public float startYOffset = -2f;

    private Rigidbody rb;
    private Collider col;
    private Vector3 startPos;
    private Vector3 finalPos;
    private bool hasRisen = false;

    void Start()
    {
        // ✅ Apply difficulty-scaled health
        if (GameManager.Instance != null)
        {
            health = GameManager.Instance.GetScaledWallHealth(baseHealth);

            // 🧠 Debug info on spawn
            Debug.Log(
                $"[WALL SPAWNED] '{name}' | Health: {health} (Base: {baseHealth}) | " +
                $"Difficulty Multiplier: {GameManager.Instance.GetDifficultyMultiplier():F2} | " +
                $"Spawn Interval: {GameManager.Instance.spawnInterval:F2}s"
            );
        }
        else
        {
            health = baseHealth;
            Debug.LogWarning($"[WALL SPAWNED] '{name}' | GameManager missing! Using base health = {baseHealth}");
        }

        rb = GetComponent<Rigidbody>() ?? GetComponentInChildren<Rigidbody>();
        col = GetComponent<Collider>() ?? GetComponentInChildren<Collider>();

        if (rb != null)
        {
            rb.useGravity = false;
            rb.constraints = RigidbodyConstraints.FreezeRotation;
        }

        if (col != null)
            col.enabled = false;

        finalPos = transform.position;
        startPos = new Vector3(finalPos.x, finalPos.y + startYOffset, finalPos.z);
        transform.position = startPos;

        GameObject playerObj = GameObject.FindWithTag("Player");
        if (playerObj != null)
            target = playerObj.transform;
    }

    void Update()
    {
        if (!hasRisen)
        {
            transform.position = Vector3.MoveTowards(transform.position, finalPos, riseSpeed * Time.deltaTime);

            if (Vector3.Distance(transform.position, finalPos) < 0.01f)
            {
                hasRisen = true;
                if (col != null) col.enabled = true;
            }
            return;
        }

        if (target == null) return;

        Vector3 direction = (target.position - transform.position).normalized;
        transform.position += direction * moveSpeed * Time.deltaTime;
        transform.LookAt(target);
    }

    public void TakeDamage(int damage)
    {
        health -= damage;

        if (health <= 0)
        {
            if (GameManager.Instance != null)
                GameManager.Instance.AddScore(scoreValue);

            Debug.Log($"[WALL DESTROYED] '{name}' | Player Score: {GameManager.Instance?.score ?? 0}");
            Destroy(gameObject);
        }
        else
        {
            Debug.Log($"[WALL HIT] '{name}' | Remaining Health: {health}");
        }
    }
}
