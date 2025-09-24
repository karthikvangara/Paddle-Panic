using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MemeManager : MonoBehaviour
{
    public static MemeManager instance;

    public bool memesEnabled = true;

    public GameObject welcome;
    public GameObject acceptingMeme;
    public GameObject beforeLogin;
    public GameObject waitingForLongTime;
    public GameObject tryingToExit;
    public GameObject healthLessThan50;
    public GameObject gameOver;

    public void Awake()
    {
        instance=this;
    }

    public void Start()
    {
        EnableWelcomeMeme();
    }

    public void EnableWelcomeMeme()
    {
        welcome.SetActive(true);
    }

    public void DisableWelcomeMeme()
    {
        welcome.SetActive(false);
    }

    public void EnableAcceptingMeme()
    {
        acceptingMeme.SetActive(true);
    }

    public void DisableAcceptingMeme()
    {
        acceptingMeme.SetActive(false);
    }


    public void EnableLoginMeme()
    {
        beforeLogin.SetActive(true);
    }

    
    public void DisableLoginMeme()
    {
        beforeLogin.SetActive(false);
    }


    public void EnableWaitingForLongTimeMeme()
    {
        waitingForLongTime.SetActive(true);
        Invoke("DisableWaitingForLongTimeMeme", 3f);
    }

    public void DisableWaitingForLongTimeMeme()
    {
        waitingForLongTime.SetActive(false);
    }

    public void EnableTryingToExit()
    {
        tryingToExit.SetActive(true);
    }
    public void DisableTryingToExit()
    {
        tryingToExit.SetActive(false);
    }

    public void EnableHealthLessThan50Meme()
    {
        healthLessThan50.SetActive(true);
    }

    public void DisableHealthLessThan50Meme()
    {
        healthLessThan50.SetActive(false);
    }

    public void EnableGameOverMeme()
    {
        gameOver.SetActive(true);
    }

    public void DisableGameOverMeme()
    {
        gameOver.SetActive(false);
    }
}
