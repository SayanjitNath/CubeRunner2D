using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PlayerScoreUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI scoreText;
    [SerializeField] private TextMeshProUGUI highScoreText;

    private void Update()
    {
        int currentScore = GameManager.Instance.GetCurrentScore();
        int highScore = GameManager.Instance.GetHighScore();

        scoreText.text = "Score : " + currentScore.ToString();
        highScoreText.text = "High Score : " + highScore.ToString();
    }
}
