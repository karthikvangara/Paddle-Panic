using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoginSceneManager : MonoBehaviour
{
    public GameObject loadingPanel;
    public GameObject memeInfoPanel;
    public GameObject memeManagerPanel;
    public GameObject loginInfoPanel;
    public string menuScene;

    public void Awake()
    {
        DontDestroyOnLoad(memeManagerPanel);
    }

    public void Update()
    {
        StartCoroutine(MemeInfo());
    }

    IEnumerator MemeInfo()
    {
        yield return new WaitForSeconds(2);
        if(MemeManager.instance!=null) MemeManager.instance.DisableWelcomeMeme();
        EnableMemeInfoPanel();
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
        if (MemeManager.instance != null) MemeManager.instance.EnableAcceptingMeme();
        StartCoroutine(LoginInfoPanel());
    }

    public void OnClickDeclineMeme()
    {
        DisableMemeInfoPanel();
        EnableLoginInfoPanel();
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
        DisableLoginInfoPanel();
        if(MemeManager.instance!=null) MemeManager.instance.DisableLoginMeme();
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
