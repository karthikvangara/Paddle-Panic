using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenuManager : MonoBehaviour
{
    public static MainMenuManager instance;

    public void Awake()
    {
        instance = this;
    }
    public void Start()
    {
        //StartCoroutine(LoadLoadingSceneInBackground());
        if (UserDataManager.instance != null) UserDataManager.instance.LoadPlayerInfo();
        if(UserDataManager.instance.userData.isFirstTime) playButton.interactable = false;
        StartCoroutine(Idle());
        LoadCollectabels();
      
    }

    private IEnumerator Idle()
    {
        yield return new WaitForSeconds(30);

        if (UserDataManager.instance != null) UserDataManager.instance.LoadPlayerInfo();
        if(MemeManager.instance != null && UserDataManager.instance.userData !=null && UserDataManager.instance.userData.isMemeAccepted) MemeManager.instance.EnableWaitingForLongTimeMeme();
        yield return new WaitForSeconds(30);
        if(MemeManager.instance != null) MemeManager.instance.DisableWaitingForLongTimeMeme();
    }

    //Collectables

    #region

    [Header("Collectables")]

    public TMP_Text collectabelsPanel;

    public void LoadCollectabels()
    {
        UserDataManager.instance.LoadPlayerInfo();
        collectabelsPanel.text = "Collectables : " + UserDataManager.instance.userData.collectablesCount.ToString();
    }

    #endregion

    //Play

    #region

    //[Header("Play / Load Scene")]

    /*private AsyncOperation asyncOp;

    private IEnumerator LoadLoadingSceneInBackground()
    {
        asyncOp = SceneManager.LoadSceneAsync("LoadingScene",LoadSceneMode.Additive);
        asyncOp.allowSceneActivation = false;
        yield return null;
    }*/

    [Header("Play")]

    public Button playButton;

    public void OnClickPlay()
    {
        //asyncOp.allowSceneActivation = true;
        //StartCoroutine(ActivateGameScene());
        SceneManager.LoadScene("LoadingScene");
    }

    public void ActivatePlayButton()
    {
        playButton.interactable = true;
    }

    /*public IEnumerator ActivateGameScene()
    {
        if (!asyncOp.isDone) yield return null;
        SceneManager.SetActiveScene(SceneManager.GetSceneByName("LoadingScene"));
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

    // Store

    #region

    public void OnClickStore()
    {
        SceneManager.LoadSceneAsync("StoreScene");
    }

    #endregion

    //Settings

    #region
    [Header("Settings")]

    public GameObject settingsPanel;
    public GameObject memesOnImage;
    public GameObject memesOffImage;

    public void OnClickSettings()
    {
        settingsPanel.SetActive(true);

        if (MemeManager.instance != null && UserDataManager.instance != null) UserDataManager.instance.LoadPlayerInfo();
        if (UserDataManager.instance.userData.isMemeAccepted)
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
        if (MemeManager.instance != null && UserDataManager.instance != null) UserDataManager.instance.LoadPlayerInfo();
        if (UserDataManager.instance.userData.isMemeAccepted)
        {
            memesOffImage.SetActive(true);
            memesOnImage.SetActive(false);
            UserDataManager.instance.userData.isMemeAccepted = false;
        }
        else
        {
            memesOffImage.SetActive(false);
            memesOnImage.SetActive(true);
            UserDataManager.instance.userData.isMemeAccepted = true;
        }

        if(UserDataManager.instance!=null) UserDataManager.instance.UpdatePlayerInfo();
    }

    #endregion

    //Exit

    #region
    [Header("Exit")]

    public GameObject ExitPanel;

    public void OnClickExit()
    {
        ExitPanel.SetActive(true);

        if(UserDataManager.instance!=null)  UserDataManager.instance.LoadPlayerInfo();
        if (MemeManager.instance != null && UserDataManager.instance.userData !=null && UserDataManager.instance.userData.isMemeAccepted) MemeManager.instance.EnableTryingToExit();
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
        //if (UserDataManager.instance == null) Debug.Log("UserDataManager is NUll");
        if (UserDataManager.instance != null)  UserDataManager.instance.LoadPlayerInfo();
        //Debug.Log(userData.playerName);
        playerNamePanel.text=UserDataManager.instance.userData.playerName;
        highScore.text=UserDataManager.instance.userData.playerScore.ToString();
        
    }

    private void ClosePlayerProfile()
    {
        animator.Play("PlayerProfileClose");
    }

    #endregion 

}
