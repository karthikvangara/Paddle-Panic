using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuManager : MonoBehaviour
{
    public void Start()
    {
        //StartCoroutine(LoadGameSceneInBackground());
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

    #region

    [Header("Play / Load Scene")]
    public string gameScene;

    /*private AsyncOperation asyncOp;

    private IEnumerator LoadGameSceneInBackground()
    {
        asyncOp = SceneManager.LoadSceneAsync(gameScene);
        asyncOp.allowSceneActivation = false;
        yield return null;
    }*/

    public void OnClickPlay()
    {
        //ActivateGameScene();
        SceneManager.LoadScene(gameScene);
    }

    /*public void ActivateGameScene()
    {
        if (asyncOp != null)
        {
            asyncOp.allowSceneActivation = true;
        }
    }*/

    #endregion

    //Multiplayer

    #region
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

    #endregion

    //Settings

    #region
    [Header("Settings")]

    public GameObject settingsPanel;
    public GameObject memesOnImage;
    public GameObject memesOffImage;
    public UserData userData;

    public void OnClickSettings()
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
    }

    public void OnCloseSettings()
    {
        settingsPanel.SetActive(false);
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

        if(UserDataManager.instance!=null) UserDataManager.instance.UpdatePlayerInfo(userData);
    }

    #endregion

    //Exit

    #region
    [Header("Exit")]

    public GameObject ExitPanel;

    public void OnClickExit()
    {
        ExitPanel.SetActive(true);
        if (MemeManager.instance != null) MemeManager.instance.EnableTryingToExit();
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
        if (MemeManager.instance != null) MemeManager.instance.DisableTryingToExit();
    }

    #endregion


    //PlayerProfile
    #region

    [Header("PlayerProfile")]

    public TMP_Text playerNamePanel;
    public TMP_Text highScore;
    public Animator animator;
    public bool isPlayerProfileOpened;

    public void OnClickPlayerProfile()
    {
        if(isPlayerProfileOpened) ClosePlayerProfile();
        if (!isPlayerProfileOpened) OpenPlayerProfile();
        isPlayerProfileOpened=!isPlayerProfileOpened;
    }

    private void OpenPlayerProfile()
    {
        animator.Play("PlayerProfileOpen");
        UserData userData = new UserData();
        //if (UserDataManager.instance == null) Debug.Log("UserDataManager is NUll");
        if (UserDataManager.instance != null) userData = UserDataManager.instance.LoadPlayerInfo();
        //Debug.Log(userData.playerName);
        playerNamePanel.text=userData.playerName;
        highScore.text=userData.playerScore.ToString();
        
    }

    private void ClosePlayerProfile()
    {
        animator.Play("PlayerProfileClose");
    }

    #endregion 

}
