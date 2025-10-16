using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TutorialManager : MonoBehaviour
{
    public GameObject tutorialManager;
    UserData userData = new UserData();

    private bool once;
    private bool inputOnlyOnceChecked;
    public void Start()
    {
        if (UserDataManager.instance != null) userData = UserDataManager.instance.LoadPlayerInfo();
        if(userData!=null && userData.isFirstTime) StartCoroutine(EnableIntroductionPanel());
    }

    public void Update()
    {
        if (userData != null && userData.isFirstTime && !once && SceneManager.GetActiveScene().name == gameScene)
        {
            CheckForGameSceneLoaded();
            once = true;
        }


        if (Movement.instance!=null && Movement.instance.isLeftPressed && !inputOnlyOnceChecked && leftSidePanel.activeSelf) StartCoroutine(EnableRightSidePanel());
        if (Movement.instance!=null && Movement.instance.isRightPressed && rightSidePanel.activeSelf && !leftSidePanel.activeSelf) StartCoroutine(EnableBothSidesPanel());
        if (Movement.instance != null && Movement.instance.isLeftPressed && Movement.instance.isRightPressed && bothSidesPanel.activeSelf && !rightSidePanel.activeSelf && !leftSidePanel.activeSelf && !inputOnlyOnceChecked) StartCoroutine(EnableHealthAndScorePanel());

    }

    // Introduction

    #region

    public GameObject introductionPanel;

    IEnumerator EnableIntroductionPanel()
    {
        yield return new WaitForSeconds(1);
        introductionPanel.SetActive(true);
    }

    public void OnClickStart()
    {
        introductionPanel.SetActive(false);
        StartCoroutine(EnablePlayerProfilePanel());
        DontDestroyOnLoad(tutorialManager);
    }

    public void OnClickSkip()
    {
        introductionPanel.SetActive(false);
        UserData userData = new UserData();
        if(UserDataManager.instance != null) userData=UserDataManager.instance.LoadPlayerInfo();
        if(userData != null && userData.isFirstTime) userData.isFirstTime = false;
        if (UserDataManager.instance != null) UserDataManager.instance.UpdatePlayerInfo(userData);
    }

    #endregion

    // PlayerProfile

    #region

    public GameObject playerProfilePanel;

    IEnumerator EnablePlayerProfilePanel()
    {
        yield return new WaitForSeconds(1);
        playerProfilePanel.SetActive(true);
    }

    public void OnClickPlayerProfileNext()
    {
        playerProfilePanel.SetActive(false);
        StartCoroutine(EnableSettingsPanel());
    }

    #endregion

    //  Settings

    #region

    public GameObject settingsPanel;

    IEnumerator EnableSettingsPanel()
    {
        yield return new WaitForSeconds(1);
        settingsPanel.SetActive(true);
    }

    public void OnClickSettingsNext()
    {
        settingsPanel.SetActive(false);
        StartCoroutine(EnableExitPanel());
    }

    #endregion

    //  Exit

    #region

    public GameObject exitPanel;

    IEnumerator EnableExitPanel()
    {
        yield return new WaitForSeconds(1);
        exitPanel.SetActive(true);
    }

    public void OnClickExitNext()
    {
        exitPanel.SetActive(false);
        StartCoroutine(EnablePlayPanel());
    }

    #endregion

    //  Play

    #region

    public GameObject playPanel;

    IEnumerator EnablePlayPanel()
    {
        yield return new WaitForSeconds(1);
        playPanel.SetActive(true);
    }

    public void OnClickPlayNext()
    {
        playPanel.SetActive(false);
    }

    #endregion

    // PlayerControls

    #region

    public GameObject leftSidePanel;
    public GameObject rightSidePanel;
    public GameObject bothSidesPanel;
    public string gameScene;

    public void CheckForGameSceneLoaded()
    {
        Debug.Log(SceneManager.GetActiveScene().name);
        StartCoroutine(EnableLeftSidePanel());
    }

    IEnumerator EnableLeftSidePanel()
    {
        yield return new WaitForSeconds(1);
        leftSidePanel.SetActive(true);
    }

    IEnumerator EnableRightSidePanel()
    {
        leftSidePanel.SetActive(false);
        yield return new WaitForSeconds(1);
        rightSidePanel.SetActive(true);
    }

    IEnumerator EnableBothSidesPanel()
    {
        rightSidePanel.SetActive(false);
        yield return new WaitForSeconds(1);
        bothSidesPanel.SetActive(true);
    }

    #endregion

    //  Health and Score

    #region

    public GameObject healthAndScorePanel;

    IEnumerator EnableHealthAndScorePanel()
    {
        //StopGame();
        bothSidesPanel.SetActive(false);
        inputOnlyOnceChecked = true;
        yield return new WaitForSeconds(1);
        healthAndScorePanel.SetActive(true);
    }

    public void OnClickHealthAndScorePanelNext()
    {
        healthAndScorePanel.SetActive(false);
        StartCoroutine(EnablePausePanel());
    }

    #endregion

    //  Pause

    #region

    public GameObject pausePanel;

    IEnumerator EnablePausePanel()
    {
        yield return new WaitForSeconds(1);
        pausePanel.SetActive(true);
    }

    public void OnClickPausePanelNext()
    {
        pausePanel.SetActive(false);
        StartCoroutine(EnableInGameSettingsPanel());
    }

    #endregion

    // Settings

    #region

    public GameObject inGameSettingsPanel;

    IEnumerator EnableInGameSettingsPanel()
    {
        yield return new WaitForSeconds(1);
        inGameSettingsPanel.SetActive(true);
    }

    public void OnClickInGameSettingsPanelNext()
    {
        inGameSettingsPanel.SetActive(false);
        StartCoroutine(EnableThanksPanel());
    }
    #endregion

    // Thanks

    #region

    public GameObject thanksPanel;

    IEnumerator EnableThanksPanel()
    {
        yield return new WaitForSeconds(1);
        thanksPanel.SetActive(true);
    }

    public void OnClickContinueGame()
    {
        thanksPanel.SetActive(false);
        //ResumeGame();
        
        UserData userData=new UserData();
        if (UserDataManager.instance != null) userData = UserDataManager.instance.LoadPlayerInfo();
        if (userData != null) userData.isFirstTime = false;
        if(UserDataManager.instance != null) UserDataManager.instance.UpdatePlayerInfo(userData);
    }

    #endregion

    public void StopGame()
    {
        Time.timeScale = 0;
    }

    public void ResumeGame()
    {
        Time.timeScale = 1;
    }

}
