using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

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
    public UserData userData;

    public void OnClickSettings()
    {
        if (!isPauseOpen)
        {
            settingsPanel.SetActive(true);

            if (MemeManager.instance != null && UserDataManager.instance != null) userData = UserDataManager.instance.LoadPlayerInfo();
            if (userData.isMemeAccepted)
            {
                memesOnImage.SetActive(true);
                memesOffImage.SetActive(false);
            }
            else
            {
                memesOnImage.SetActive(false);
                memesOffImage.SetActive(true);
            }
            isSettingsOpen = true;
        }
    }

    public void OnCloseSettings()
    {
        isSettingsOpen=false;
        settingsPanel.SetActive(false);
        ResumeGame();
    }


    public void OnClickMemesControl()
    {
        if (MemeManager.instance != null && UserDataManager.instance != null) userData = UserDataManager.instance.LoadPlayerInfo();
        if (userData.isMemeAccepted)
        {
            memesOffImage.SetActive(true);
            memesOnImage.SetActive(false);
            userData.isMemeAccepted = false;
        }
        else
        {
            memesOffImage.SetActive(false);
            memesOnImage.SetActive(true);
            userData.isMemeAccepted = true;
        }

        if (UserDataManager.instance != null) UserDataManager.instance.UpdatePlayerInfo(userData);
    }

    #endregion

    //GameOverPanel

    #region

    [Header("GameOver")]

    public Movement movement;
    public GameObject gameOverPanel;
    public TMP_Text score;
    public TMP_Text highScore;
    public int currScore;


    public void OpenGameOverPanel()
    {
        isPauseOpen = true;
        StartCoroutine(DisplayGameOverPanel());
    }

    IEnumerator DisplayGameOverPanel()
    {
        yield return new WaitForSeconds(3f);
        gameOverPanel.SetActive(true); 
        score.text = scorePanel.text;
        //Debug.Log("Curr Score " + currScore);

        UserData userData = new UserData();
        if (UserDataManager.instance != null) userData = UserDataManager.instance.LoadPlayerInfo();

        //Debug.Log("High Score " + userData.playerScore);
        if (currScore > userData.playerScore)
        {
            userData.playerScore = currScore;
            if (UserDataManager.instance != null) UserDataManager.instance.UpdatePlayerInfo(userData);

        }
        highScore.text = userData.playerScore.ToString();
        if (MemeManager.instance != null && userData != null && userData.isMemeAccepted) MemeManager.instance.EnableGameOverMeme();
        StartCoroutine(DisableGameOverMeme());
    }

    IEnumerator DisableGameOverMeme()
    {
        yield return new WaitForSeconds(3);
        if (MemeManager.instance != null) MemeManager.instance.DisableGameOverMeme();
        StopGame();
    }

    #endregion

    //HealthPanel

    #region

    [Header("Health")]

    public TMP_Text health;
    //public Image healthRef;
    public HealthManager healthManager;
    public float maxAlpha = 1f;
    public float healthCoefficient = 0.5f;
    public bool once = true;

    public void UpdateHealth()
    {
        health.text=Mathf.Max(0,System.Convert.ToInt32(healthManager.currHealth)).ToString();
        //float healthPrecentage = (healthManager.currHealth / healthManager.maxHealth);
        //Debug.Log(healthPrecentage);
        //float alphaValue = Mathf.Lerp(maxAlpha, 0f, healthPrecentage);
        //healthRef.color = new Color(1f, 0f, 0f, alphaValue*healthCoefficient);

        if (healthManager.currHealth < 50f && once)
        {
            UserData userData=new UserData();
            if (UserDataManager.instance != null) userData = UserDataManager.instance.LoadPlayerInfo();
            if (MemeManager.instance != null && userData!=null && userData.isMemeAccepted) MemeManager.instance.EnableHealthLessThan50Meme();
            StartCoroutine(DisableHealthLessThan50Meme());
        }

    }

    IEnumerator DisableHealthLessThan50Meme()
    {
        yield return new WaitForSeconds(2);
        if (MemeManager.instance != null) MemeManager.instance.DisableHealthLessThan50Meme();
        once = false;
    }

    #endregion

    //ScorePanel

    #region

    [Header("Score")]
    public TMP_Text scorePanel;

    public void UpdateScore()
    {
        currScore=System.Convert.ToInt32(movement.score);
        scorePanel.text=currScore.ToString();
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
