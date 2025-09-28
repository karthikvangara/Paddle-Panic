using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Text.RegularExpressions;
using TMPro;

public class LoginSceneManager : MonoBehaviour
{
    public GameObject loadingPanel;
    public GameObject memeInfoPanel;
    public GameObject memeManagerPanel;
    public GameObject loginInfoPanel;
    public GameObject loginInputFieldError;
    public GameObject UserDataManagerObject;
    public GameObject AudioManager;
    public TMP_InputField playerName;
    public string menuScene;

    public UserData userData;

    public void Awake()
    {
        DontDestroyOnLoad(memeManagerPanel);
        DontDestroyOnLoad(UserDataManagerObject);
        DontDestroyOnLoad(AudioManager);
        userData=new UserData();
    }

    public void Update()
    {
        if(UserDataManager.instance!=null) userData=UserDataManager.instance.LoadPlayerInfo();
        //if (userData == null) Debug.Log("User Data is Null");
        StartCoroutine(MemeInfo());
        
    }

    IEnumerator MemeInfo()
    {
        //Debug.Log("Meme Info Coroutine called");
        yield return new WaitForSeconds(2);
        if(MemeManager.instance!=null) MemeManager.instance.DisableWelcomeMeme();

        if (userData != null && !userData.isFirstTime) LoadMainMenuScene();
        else EnableMemeInfoPanel();
    }

    public void EnableMemeInfoPanel()
    {
        memeInfoPanel.SetActive(true);
    }

    public void DisableMemeInfoPanel()
    {
        memeInfoPanel.SetActive(false);
    }

    public void OnClickAcceptMeme()
    {
        DisableMemeInfoPanel();
        
        UserData userData = new UserData();
        userData.isMemeAccepted = true;
        if (UserDataManager.instance != null) UserDataManager.instance.UpdatePlayerInfo(userData);

        if (MemeManager.instance != null) MemeManager.instance.EnableAcceptingMeme();
        StartCoroutine(LoginInfoPanel());
    }

    public void OnClickDeclineMeme()
    {
        DisableMemeInfoPanel();
        EnableLoginInfoPanel();

        UserData userData = new UserData();
        userData.isMemeAccepted = false;
        if (UserDataManager.instance != null) UserDataManager.instance.UpdatePlayerInfo(userData);

        if (MemeManager.instance != null) MemeManager.instance.memesEnabled = false;
        memeManagerPanel.SetActive(false);
    }

    IEnumerator LoginInfoPanel()
    {
        yield return new WaitForSeconds(2);
        if (MemeManager.instance!=null) MemeManager.instance.DisableAcceptingMeme();
        EnableLoginInfoPanel();
    }

    public void EnableLoginInfoPanel()
    {
        loginInfoPanel.SetActive(true);
        if(MemeManager.instance!=null) MemeManager.instance.EnableLoginMeme();
    }

    public void DisableLoginInfoPanel()
    {
        loginInfoPanel.SetActive(false);
    }

    public void OnClickLogin()
    {
        string playerNameWithoutSpaces = Regex.Replace(playerName.text, @"\s+", "");
        if (playerNameWithoutSpaces.Length > 0)
        {
            if (userData != null) userData.playerName = playerName.text;
            if(UserDataManager.instance!=null) UserDataManager.instance.UpdatePlayerInfo(userData);
            DisableLoginInfoPanel();
            if (MemeManager.instance != null) MemeManager.instance.DisableLoginMeme();
            LoadMainMenuScene();
        }
        else
        {
            loginInputFieldError.SetActive(true);
        }
    }


    public void LoadMainMenuScene()
    {
        if (UserDataManager.instance != null) UserDataManager.instance.UpdatePlayerInfo(userData);
        SceneManager.LoadScene(menuScene);
    }
    /*public void DisableLoadingUIAndEnableAcceptingMemeUI()
    {
        loadingPanel.SetActive(false);
        memeInfoPanel.SetActive(true);
    }

    public void OnClickAcceptMeme()
    {
        memeInfoPanel.SetActive(false);
        loginInfoPanel.SetActive(true);
        DontDestroyOnLoad(memeManagerPanel);
    }

    public void OnClickDeclineMeme()
    {
        memeInfoPanel.SetActive(false);
        loginInfoPanel.SetActive(true);
        memeManagerPanel.SetActive(false);
    }

    public void OnClickLogin()
    {
        SceneManager.LoadScene(menuScene);
    }*/
}
