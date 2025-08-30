using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MemeManager : MonoBehaviour
{
    public static MemeManager instance;
    public AudioSource beforeAcceptingMeme;
    public AudioSource acceptingMeme;
    public AudioSource beforeLogin;
    public AudioSource welcome;
    public AudioSource waitingForLongTime;
    public AudioSource hittingObstacles;
    public AudioSource crash;

    public void Awake()
    {
        instance=this;
    }

    public void Start()
    {
        Invoke("BeforeAcceptingMeme", 0.5f);
    }
    public void BeforeAcceptingMeme()
    {
        beforeAcceptingMeme.Play();
    }

    public void AcceptingMeme()
    {
        acceptingMeme.Play();
        Invoke("BeforeLogin", 1.5f);
    }

    public void BeforeLogin()
    {
        beforeLogin.Play();
    }


    public void Welcome()
    {
        welcome.Play();
    }

    public void WaitingForLongTime()
    {
        waitingForLongTime.Play();
    }

    public void HittingObstacles()
    {
        hittingObstacles.Play();
    }

    public void Crashed()
    {
        crash.Play();
    }
}
