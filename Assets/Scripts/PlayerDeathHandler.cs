using UnityEngine;
using UnityEngine.SceneManagement;

public class DeathZone : MonoBehaviour
{
    [Header("References")]
    [Tooltip("Assign your Game Over Canvas here in the Inspector.")]
    public GameObject deathUI;

    private bool gameOverTriggered = false;

    void Start()
    {
        // ✅ Ensure Death UI is hidden at start
        if (deathUI != null)
            deathUI.SetActive(false);
        else
            Debug.LogWarning("⚠ Death UI not assigned in DeathZone!");
    }

    void OnTriggerEnter(Collider other)
    {
        // 🔒 Prevent duplicate triggers
        if (gameOverTriggered)
            return;

        // 🎯 Detect walls by tag or layer (more robust)
        if (other.CompareTag("Wall") || other.gameObject.layer == LayerMask.NameToLayer("Wall"))
        {
            TriggerDeath();
        }
    }

    private void TriggerDeath()
    {
        gameOverTriggered = true;

        // 🧊 Freeze game
        Time.timeScale = 0f;

        // 💀 Show death screen instantly
        if (deathUI != null)
            deathUI.SetActive(true);
        else
            Debug.LogWarning("⚠ Death UI not assigned — cannot display!");

        // 🖱 Unlock cursor
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        Debug.Log("💀 Game Over triggered!");
    }

    void Update()
    {
        // 🖱 If player clicks anywhere, restart to Main Menu
        if (gameOverTriggered && Input.GetMouseButtonDown(0))
        {
            Time.timeScale = 1f;
            SceneManager.LoadScene(0);
        }
    }
}
