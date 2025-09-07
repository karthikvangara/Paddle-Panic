using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameUIManager : MonoBehaviour
{
    public string mainMenuSceneName;

    private bool isPauseOpen;
    private bool isSettingsOpen;

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
    
    public void StopGame()
    {
        Time.timeScale = 0f;
    }

    public void ResumeGame()
    {
        Time.timeScale = 1f;
    }
}
