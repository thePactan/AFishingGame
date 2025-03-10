using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIManager : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField]
    private TextMeshProUGUI _countdownText, _scoreText, _bestScoreText;

    private GameManager _gameManager;

    public int Score, BestScore;

    public void UpdateCountdownText(float time)
    {
        int minutes = Mathf.FloorToInt(time / 60F);
        int seconds = Mathf.FloorToInt(time % 60F);
        _countdownText.text = string.Format("{0:00}:{1:00}", minutes, seconds);
        if (time < 60)
        {
            _countdownText.color = Color.red;
        }
        else
        {
            _countdownText.color = Color.white;
        }
    }

    public void UpdateScore(int playerScore)
    {
        if (_scoreText == null)
        {
            Debug.LogError("ScoreText is not assigned!");
            return;
        }
        _scoreText.text = "Score: " + playerScore.ToString();
    }

    public void UpdateBestScore(int bestScore)
    {
        if (_bestScoreText == null)
        {
            Debug.LogError("BestScoreText is not assigned!");
            return;
        }
        _bestScoreText.text = "Best Score: " + bestScore.ToString();

    }
}
