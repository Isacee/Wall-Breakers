using UnityEngine;

public class Bullet : MonoBehaviour
{
    [Header("Base Settings")]
    public float speed = 20f;
    public float lifetime = 5f;
    public int damage = 25;
    public int maxBounces = 3;

    private Rigidbody rb;
    private int bounceCount = 0;

    // 🔥 Static variables store permanent upgrades across all bullets
    private static float permanentSpeedBonus = 0f;
    private static int permanentDamageBonus = 0;
    private static int permanentMaxBouncesBonus = 0;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.useGravity = false;
        rb.collisionDetectionMode = CollisionDetectionMode.Continuous;

        // Apply permanent upgrades to each new bullet
        float finalSpeed = speed + permanentSpeedBonus;
        int finalDamage = damage + permanentDamageBonus;
        int finalMaxBounces = maxBounces + permanentMaxBouncesBonus;

        rb.linearVelocity = transform.forward * finalSpeed;

        // Log creation info
        Debug.Log($"🆕 Bullet spawned: Damage={finalDamage}, Speed={finalSpeed}, MaxBounces={finalMaxBounces}");

        // Set local stats
        speed = finalSpeed;
        damage = finalDamage;
        maxBounces = finalMaxBounces;

        Destroy(gameObject, lifetime);
    }

    void OnCollisionEnter(Collision collision)
    {
        // ✅ Wall detection and damage
        Wall wall = collision.gameObject.GetComponentInParent<Wall>();
        if (wall != null)
        {
            wall.TakeDamage(damage);
            Debug.Log($"💥 Hit wall! {wall.name} took {damage} damage.");
        }

        // ✅ Destroy on ground hit
        if (collision.gameObject.CompareTag("Ground"))
        {
            Destroy(gameObject);
            return;
        }

        // ✅ Bounce reflection
        if (collision.contacts.Length > 0)
        {
            Vector3 reflectDir = Vector3.Reflect(rb.linearVelocity.normalized, collision.contacts[0].normal);
            rb.linearVelocity = reflectDir * speed;
        }

        bounceCount++;

        // 🎯 Permanent upgrades based on bounces
        if (bounceCount == 2)
        {
            // 2nd bounce → permanently increase damage
            permanentDamageBonus += 5;
            Debug.Log($"🔥 Permanent Damage Bonus Increased! Now +{permanentDamageBonus}");
        }
        else if (bounceCount == 3)
        {
            // 3rd bounce → permanently increase speed
            permanentSpeedBonus += 2f;
            Debug.Log($"⚡ Permanent Speed Bonus Increased! Now +{permanentSpeedBonus}");
        }
        else if (bounceCount > maxBounces)
        {
            // Reached max bounce → permanently increase max bounces
            permanentMaxBouncesBonus++;
            bounceCount = 0;
            Debug.Log($"🚀 Permanent Max Bounces Increased! Now +{permanentMaxBouncesBonus}");
        }
    }
}
