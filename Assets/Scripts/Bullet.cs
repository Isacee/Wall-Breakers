using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float speed = 20f;
    public float lifetime = 5f;
    public int damage = 25;
    public int maxBounces = 3;

    private Rigidbody rb;
    private int bounceCount = 0;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.useGravity = false;
        rb.collisionDetectionMode = CollisionDetectionMode.Continuous;
        rb.linearVelocity = transform.forward * speed;
        Destroy(gameObject, lifetime);
    }

    void OnCollisionEnter(Collision collision)
    {
        // 🔍 Try to find a Wall script on the hit object or its parent
        Wall wall = collision.gameObject.GetComponentInParent<Wall>();

        if (wall != null)
        {
            wall.TakeDamage(damage);
            Debug.Log($"Hit wall! {wall.name} took {damage} damage.");
        }

        // ✅ Bounce logic
        Vector3 reflectDir = Vector3.Reflect(rb.linearVelocity.normalized, collision.contacts[0].normal);
        rb.linearVelocity = reflectDir * speed;

        bounceCount++;
        if (bounceCount >= maxBounces)
        {
            Destroy(gameObject);
        }

        // ✅ Destroy bullet if it hits ground or something else
        if (collision.gameObject.CompareTag("Ground"))
        {
            Destroy(gameObject);
        }
    }
}
