using UnityEngine;

public class Wall : MonoBehaviour
{
    public Transform target;
    public float moveSpeed = 2f;
    public int health = 100;

    [Header("Rising Settings")]
    public float riseHeight = 3f;
    public float riseSpeed = 2f;
    public float startYOffset = -2f;

    private Rigidbody rb;
    private Collider col;
    private Vector3 startPos;
    private Vector3 finalPos;
    private bool hasRisen = false;

    void Start()
    {
        // ? Automatically find Rigidbody and Collider (even if on child)
        rb = GetComponent<Rigidbody>();
        if (rb == null)
            rb = GetComponentInChildren<Rigidbody>();

        col = GetComponent<Collider>();
        if (col == null)
            col = GetComponentInChildren<Collider>();

        if (rb != null)
        {
            rb.useGravity = false;
            rb.constraints = RigidbodyConstraints.FreezeRotation;
        }

        // Disable collider while rising
        if (col != null)
            col.enabled = false;

        // Set positions
        finalPos = transform.position;
        startPos = new Vector3(finalPos.x, finalPos.y + startYOffset, finalPos.z);
        transform.position = startPos;

        // Find player target
        GameObject playerObj = GameObject.FindWithTag("Player");
        if (playerObj != null)
        {
            target = playerObj.transform;
        }
    }

    void Update()
    {
        if (!hasRisen)
        {
            transform.position = Vector3.MoveTowards(transform.position, finalPos, riseSpeed * Time.deltaTime);

            if (Vector3.Distance(transform.position, finalPos) < 0.01f)
            {
                hasRisen = true;
                if (col != null)
                    col.enabled = true; // ? turn collider back on
            }
            return;
        }

        if (target == null) return;

        // Move toward player
        Vector3 direction = (target.position - transform.position).normalized;
        transform.position += direction * moveSpeed * Time.deltaTime;
        transform.LookAt(target);
    }

    public void TakeDamage(int damage)
    {
        health -= damage;
        if (health <= 0)
            Destroy(gameObject);
    }
}
