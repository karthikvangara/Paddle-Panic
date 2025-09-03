using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuManager : MonoBehaviour
{
    public float waitMemeTime=10f;

    private bool once;
    private float waitMemeTimer;
    public void Start()
    {
        StartCoroutine(LoadGameSceneInBackground());
        WelcomeMeme();
    }

    public void Update()
    {
        if (waitMemeTimer > waitMemeTime && !once)
        {
            WaitingForLongTimeMeme();
            once = true;
        }
        waitMemeTimer += Time.deltaTime;
    }

    //Play

    [Header("Play / Load Scene")]
    public string gameScene;

    private AsyncOperation asyncOp;

    private IEnumerator LoadGameSceneInBackground()
    {
        asyncOp = SceneManager.LoadSceneAsync(gameScene);
        asyncOp.allowSceneActivation = false;
        yield return null;
    }

    public void OnClickPlay()
    {
        ActivateGameScene();
    }

    public void ActivateGameScene()
    {
        if (asyncOp != null)
        {
            asyncOp.allowSceneActivation = true;
        }
    }

    //Multiplayer
    [Header("Multiplayer")]

    public GameObject multiplayerPanel;

    public void OnClickMultiplayer()
    {
        multiplayerPanel.SetActive(true);
    }

    public void OnCloseMultiplayer()
    {
        multiplayerPanel.SetActive(false);
    }


    //Settings
    [Header("Settings")]

    public GameObject settingsPanel;

    public void OnClickSettings()
    {
        settingsPanel.SetActive(true);
    }

    public void OnCloseSettings()
    {
        settingsPanel.SetActive(false);
    }


    //Exit
    [Header("Exit")]

    public GameObject ExitPanel;

    public void OnClickExit()
    {
        ExitPanel.SetActive(true);
    }

    public void OnClickYes()
    {
        Application.Quit();

        // This is only for testing inside the Unity Editor
        #if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
        #endif
    }

    public void OnCloseExit()
    {
        ExitPanel.SetActive(false);
    }

    //Memes

    public void WelcomeMeme()
    {
        MemeManager.instance.Welcome();
    }

    public void WaitingForLongTimeMeme()
    {
        MemeManager.instance.WaitingForLongTime();
    }
}
