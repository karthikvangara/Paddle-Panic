using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameUIManager : MonoBehaviour
{
    public string mainMenuSceneName;

    private bool isPauseOpen;
    private bool isSettingsOpen;

    //Pause Panel

    #region

    [Header("Pause")]
    public GameObject pausePanel;
    public void OnClickPause()
    {
        if (!isSettingsOpen)
        {
            pausePanel.SetActive(true);
            StopGame();
            isPauseOpen = true;
        }
    }

    public void OnClickResume()
    {
        pausePanel.SetActive(false);
        ResumeGame();
        isPauseOpen = false;
    }

    public void OnClickRestart()
    {
        isPauseOpen = false;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        ResumeGame();
    }

    public void OnClickExit()
    {
        SceneManager.LoadScene(mainMenuSceneName);
    }

    #endregion

    //Settings Panel

    #region

    [Header("Settings")]

    public GameObject settingsPanel;
    public GameObject memesOnImage;
    public GameObject memesOffImage;

    public void OnClickSettings()
    {
        if (!isPauseOpen)
        {
            settingsPanel.SetActive(true);
            if (MemeManager.instance != null && !MemeManager.instance.memesEnabled) OnClickDisableMemes();
            StopGame();
            isSettingsOpen = true;
        }
    }

    public void OnCloseSettings()
    {
        isSettingsOpen=false;
        settingsPanel.SetActive(false);
        ResumeGame();
    }


    public void OnClickEnableMemes()
    {
        if (MemeManager.instance != null) MemeManager.instance.gameObject.SetActive(true);
        if(MemeManager.instance !=null) MemeManager.instance.memesEnabled = true;
        memesOnImage.SetActive(true);
        memesOffImage.SetActive(false);
    }

    public void OnClickDisableMemes()
    {
        if (MemeManager.instance != null) MemeManager.instance.gameObject.SetActive(false); 
        if (MemeManager.instance != null) MemeManager.instance.memesEnabled = false;
        memesOnImage.SetActive(false);
        memesOffImage.SetActive(true);
    }

    #endregion

    //GameOverPanel

    #region

    [Header("GameOver")]

    public Movement movement;
    public GameObject gameOverPanel;
    public TMP_Text score;
    public TMP_Text highScore;


    public void OpenGameOverPanel()
    {
        StopGame();
        isPauseOpen = true;
        gameOverPanel.SetActive(true);
        int currScore = System.Convert.ToInt32(movement.score);
        score.text=currScore.ToString();
        Debug.Log("Curr Score "+currScore);

        UserData userData = new UserData();
        if (UserDataManager.instance != null) userData = UserDataManager.instance.LoadPlayerInfo();

        Debug.Log("High Score " + userData.playerScore);
        if (currScore > userData.playerScore)
        {
            userData.playerScore = currScore;
            if (UserDataManager.instance != null) UserDataManager.instance.UpdatePlayerInfo(userData);

        }
        highScore.text=userData.playerScore.ToString();
    }

    #endregion

    public void StopGame()
    {
        Time.timeScale = 0f;
    }

    public void ResumeGame()
    {
        Time.timeScale = 1f;
    }
}
