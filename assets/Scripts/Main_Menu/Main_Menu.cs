using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class Main_Menu : MonoBehaviour
{
    public GameObject settingsMenu;
    public GameObject mainMenu;
    [SerializeField]
    private TextMeshProUGUI _bestScoreText;

    void Start()
    {
        int bestScore = PlayerPrefs.GetInt("BestScore", 0);
        UpdateBestScoreText(bestScore);
        SwitchToMainMenu();
    }

    public void StartGame()
    {
        SceneManager.LoadScene("Main_Game");

    }

    public void SettingMenu()
    {
        mainMenu.SetActive(false);
        settingsMenu.SetActive(true);
    }

    public void Exit()
    {
        Debug.Log("Exiting game...");
        Application.Quit();
    }
    public void ResetBestScore()
    {
        PlayerPrefs.SetInt("BestScore", 0);
        PlayerPrefs.Save();
        Debug.Log("Reset Best Score To Zero");
        UpdateBestScoreText(0);
    }
    public void SwitchToMainMenu()
    {
        settingsMenu.SetActive(false);
        mainMenu.SetActive(true);
    }

    private void UpdateBestScoreText(int bestScore)
    {
        _bestScoreText.text = "Best Score: " + bestScore.ToString();
    }
}
