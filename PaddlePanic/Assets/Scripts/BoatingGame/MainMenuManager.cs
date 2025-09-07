using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuManager : MonoBehaviour
{
    public void Start()
    {
        StartCoroutine(LoadGameSceneInBackground());
        StartCoroutine(Idle());
    }

    private IEnumerator Idle()
    {
        yield return new WaitForSeconds(30);
        if(MemeManager.instance != null) MemeManager.instance.EnableWaitingForLongTimeMeme();
        yield return new WaitForSeconds(30);
        if(MemeManager.instance != null) MemeManager.instance.DisableWaitingForLongTimeMeme();
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
    public GameObject memesOnImage;
    public GameObject memesOffImage;

    public void OnClickSettings()
    {
        settingsPanel.SetActive(true);
        if (MemeManager.instance!=null && !MemeManager.instance.memesEnabled) OnClickDisableMemes();
    }

    public void OnCloseSettings()
    {
        settingsPanel.SetActive(false);
    }

    public void OnClickEnableMemes()
    {
        if(MemeManager.instance!=null) MemeManager.instance.gameObject.SetActive(true);
        if (MemeManager.instance != null) MemeManager.instance.memesEnabled = true;
        memesOnImage.SetActive(true);
        memesOffImage.SetActive(false);
    }

    public void OnClickDisableMemes()
    {
        if(MemeManager.instance!=null) MemeManager.instance.gameObject.SetActive(false);
        if (MemeManager.instance != null) MemeManager.instance.memesEnabled = false;
        memesOnImage.SetActive(false);
        memesOffImage.SetActive(true);
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

}
