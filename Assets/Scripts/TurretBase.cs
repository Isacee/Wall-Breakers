using UnityEngine;

public class TurretController : MonoBehaviour
{
    [Header("Aiming")]
    public Camera mainCamera;
    public float rotationSpeed = 10f;
    public float heightOffset = 0f;

    [Header("Shooting")]
    public Transform muzzlePoint;
    public GameObject bulletPrefab;
    public float bulletSpeed = 20f;
    public float fireRate = 0.25f; // seconds between shots

    private float nextFireTime = 0f;

    void Update()
    {
        AimAtMouse();

        if (Input.GetMouseButton(0) && Time.time >= nextFireTime)
        {
            Shoot();
            nextFireTime = Time.time + fireRate;
        }
    }

    void AimAtMouse()
    {
        if (!mainCamera) mainCamera = Camera.main;

        Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
        Plane groundPlane = new Plane(Vector3.up, Vector3.zero);

        if (groundPlane.Raycast(ray, out float hitDist))
        {
            Vector3 hitPoint = ray.GetPoint(hitDist);
            hitPoint.y = transform.position.y + heightOffset;

            Vector3 direction = hitPoint - transform.position;
            if (direction.sqrMagnitude > 0.001f)
            {
                Quaternion targetRot = Quaternion.LookRotation(direction);
                transform.rotation = Quaternion.Slerp(
                    transform.rotation,
                    targetRot,
                    Time.deltaTime * rotationSpeed
                );
            }
        }
    }

    void Shoot()
    {
        if (bulletPrefab && muzzlePoint)
        {
            GameObject bullet = Instantiate(bulletPrefab, muzzlePoint.position, muzzlePoint.rotation);
            Rigidbody rb = bullet.GetComponent<Rigidbody>();

            if (rb != null)
            {
                rb.linearVelocity = muzzlePoint.forward * bulletSpeed;
            }

            // Auto destroy bullet after 3 seconds
            Destroy(bullet, 3f);
        }
    }
}
