using UnityEngine;

[RequireComponent(typeof(Collider))]
public class SolidObject : MonoBehaviour
{
    [Header("Physics Settings")]
    public bool useGravity = false;
    public bool isKinematic = false;
    public bool freezeRotation = true;

    private Rigidbody rb;

    void Awake()
    {
        rb = GetComponent<Rigidbody>();

        // ? Add Rigidbody if missing
        if (rb == null)
        {
            rb = gameObject.AddComponent<Rigidbody>();
        }

        // ? Configure Rigidbody
        rb.useGravity = useGravity;
        rb.isKinematic = isKinematic;

        if (freezeRotation)
            rb.constraints = RigidbodyConstraints.FreezeRotation;

        // ? Ensure the collider is solid
        Collider col = GetComponent<Collider>();
        col.isTrigger = false;
    }

    // Optional: simple debug check
    void OnCollisionEnter(Collision collision)
    {
        Debug.Log($"{name} collided with {collision.gameObject.name}");
    }
}
