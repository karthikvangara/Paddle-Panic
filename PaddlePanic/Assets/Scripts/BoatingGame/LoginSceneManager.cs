using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoginSceneManager : MonoBehaviour
{
    public GameObject memeInfo;
    public GameObject memeManager;
    public GameObject loginInfo;
    public string menuScene;

    public void OnClickAcceptMeme()
    {
        memeInfo.SetActive(false);
        loginInfo.SetActive(true);
        DontDestroyOnLoad(memeManager);
    }

    public void OnClickDeclineMeme()
    {
        memeInfo.SetActive(false);
        loginInfo.SetActive(true);
        memeManager.SetActive(false);
    }

    public void OnClickLogin()
    {
        SceneManager.LoadScene(menuScene);
    }
}
