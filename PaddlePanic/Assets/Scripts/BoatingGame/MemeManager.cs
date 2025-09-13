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
    public GameObject hittingObstacles;
    public GameObject crash;

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

    public void EnableHittingObstaclesMeme()
    {
        hittingObstacles.SetActive(true);
    }

    public void DisableHittingObstaclesMeme()
    {
        hittingObstacles.SetActive(false);
    }

    public void EnableCrashedMeme()
    {
        crash.SetActive(true);
    }

    public void DisableCrashedMeme()
    {
        crash.SetActive(false);
    }
}
