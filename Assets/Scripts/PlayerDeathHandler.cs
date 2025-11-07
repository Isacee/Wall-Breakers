using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerDeathHandler : MonoBehaviour
{
    [SerializeField] private YouDiedUI deathUI;

    private bool isDead = false;

    private void OnCollisionEnter(Collision collision)
    {
        if (isDead) return;

        // Tag your walls as “Wall”
        if (collision.gameObject.CompareTag("Wall"))
        {
            StartCoroutine(HandleDeath());
        }
    }

    private IEnumerator HandleDeath()
    {
        isDead = true;

        Time.timeScale = 0f; // Stops the game

        Time.timeScale = 1f;

        yield return StartCoroutine(deathUI.PlayDeathSequence(() =>
        {
            Time.timeScale = 0f;
        }));

        yield return new WaitForSecondsRealtime(0.5f);

        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
