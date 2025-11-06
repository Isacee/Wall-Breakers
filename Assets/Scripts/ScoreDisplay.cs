using UnityEngine;
using TMPro; 

public class ScoreDisplay : MonoBehaviour
{
    public TMP_Text scoreText;

    void Update()
    {
        if (GameManager.Instance != null)
            scoreText.text = "Score: " + GameManager.Instance.score;
    }
}
