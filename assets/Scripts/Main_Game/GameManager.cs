using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField]
    private float _countdownTime = 300f;
    private float _currentTime;

    [SerializeField]
    private UIManager _uiManager;
    [SerializeField]
    private GameObject _pauseMenuPanel;
    [SerializeField]
    private GameObject _gameOverPanel;
    [SerializeField]
    private SpawnManager _spawnManager;
    private bool _isGameStart = false;
    private bool _isGameOver = false;
    private bool _isGamePaused = true;
    private int _score = 0;
    private void Start()
    {
        _currentTime = _countdownTime;
        _uiManager = FindObjectOfType<UIManager>();
        _spawnManager = FindObjectOfType<SpawnManager>();

        if (_uiManager == null)
        {
            Debug.LogError("UIManager not found!");
        }

        if (_spawnManager == null)
        {
            Debug.LogError("SpawnManager not found!");
        }

        int bestScore = PlayerPrefs.GetInt("BestScore", 0);
        _uiManager.UpdateBestScore(bestScore);
    }


    private void Update()
    {
        if (!_isGameOver)
        {

            if (Input.GetKeyDown(KeyCode.Space) && !_isGameStart)
            {
                StartGame();
                _isGameStart = true;
            }

            if (Input.GetKeyDown(KeyCode.Escape))
            {
                if (_isGamePaused)
                {
                    ResumeGame();
                }
                else
                {
                    PauseGame();
                }
            }
        }
    }

    private void StartGame()
    {
        StartCoroutine(CountdownTimer());
        _isGamePaused = false;
        _spawnManager._isSpawning = true;
        Time.timeScale = 1f;
        //Debug.Log("Game Started");
    }


    private void PauseGame()
    {
        _pauseMenuPanel.SetActive(true);
        Time.timeScale = 0f;
        _isGamePaused = true;
        //Debug.Log("Game Paused");
    }

    public void ResumeGame()
    {
        _pauseMenuPanel.SetActive(false);
        Time.timeScale = 1f;
        _isGamePaused = false;
        //Debug.Log("Game Resumed");
    }



    private void GameOver()
    {
        _isGameOver = true;
        _spawnManager._isSpawning = false;
        _gameOverPanel.SetActive(true);
        Time.timeScale = 0f;
        //Debug.Log("Game Over!!");
    }

    public void RestartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void BacktoMainMenu()
    {
        SceneManager.LoadScene("Main_Menu");
    }

    public void SaveBestScore(int bestScore)
    {
        PlayerPrefs.SetInt("BestScore", bestScore);
        PlayerPrefs.Save(); // Ensure the data is saved
    }

    public void AddScore(int points)
    {
        _score += points;
        _uiManager.UpdateScore(_score);

        CheckForBestScore(_score);
    }

    private void CheckForBestScore(int playerScore)
    {
        int bestScore = PlayerPrefs.GetInt("BestScore", 0);
        Debug.Log("CheckForBestScore get call" + bestScore);
        if (playerScore > bestScore)
        {
            PlayerPrefs.SetInt("BestScore", playerScore);
            PlayerPrefs.Save();
            _uiManager.UpdateBestScore(playerScore);
            Debug.Log("New best score: " + playerScore);
        }
    }


    IEnumerator CountdownTimer()
    {
        while (_currentTime > 0)
        {
            yield return new WaitForSeconds(1f);
            _currentTime--;
            _uiManager.UpdateCountdownText(_currentTime);
        }
        GameOver();
    }
}
