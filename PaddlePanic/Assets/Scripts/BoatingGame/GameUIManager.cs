using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameUIManager : MonoBehaviour
{
    public string mainMenuSceneName;
    public GameObject pausePanel;
    public void OnClickPause()
    {
        pausePanel.SetActive(true);
        Time.timeScale = 0f;
    }

    public void OnClickResume()
    {
        pausePanel.SetActive(false);
        Time.timeScale = 1f;
    }

    public void OnClickExit()
    {
        SceneManager.LoadScene(mainMenuSceneName);
    }
}
