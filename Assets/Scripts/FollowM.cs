using UnityEngine;
using UnityEngine.InputSystem;

public class FPSMouseCamera : MonoBehaviour
{
    [Header("References")]
    public Transform turretBase;   // Assign your TurretBase here
    public float sensitivity = 2f;
    public float heightOffset = 1.5f;
    public float distance = 0.5f;
    public float smoothFollow = 10f;

    private float xRotation = 0f; // pitch

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void LateUpdate()
    {
        if (Mouse.current == null) return;

        // --- Mouse movement ---
        Vector2 mouseDelta = Mouse.current.delta.ReadValue() * sensitivity;

        // Vertical rotation (pitch)
        xRotation -= mouseDelta.y;
        xRotation = Mathf.Clamp(xRotation, -45f, 60f);

        // Horizontal rotation (yaw)
        turretBase.Rotate(Vector3.up * mouseDelta.x);

        // --- Camera rotation ---
        Quaternion camRot = Quaternion.Euler(xRotation, turretBase.eulerAngles.y, 0f);
        transform.rotation = camRot;

        // --- Camera position ---
        Vector3 desiredPos = turretBase.position
            + Vector3.up * heightOffset
            - transform.forward * distance;

        transform.position = Vector3.Lerp(transform.position, desiredPos, smoothFollow * Time.deltaTime);
    }
}
