using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthUIManager : MonoBehaviour
{
    public GameObject fullHealth;
    public GameObject _80perHealth;
    public GameObject _60perHealth;
    public GameObject _40perHealth;
    public GameObject _20perHealth;
    public GameObject _0perHealth;

    public void Display100PerHealth()
    {
        fullHealth.SetActive(true);
    }
    public void Display80PerHealth()
    {
        _80perHealth.SetActive(true);
    }

    public void Display60PerHealth()
    {
        _60perHealth.SetActive(true);
    }

    public void Display40PerHealth()
    {
        _40perHealth.SetActive(true);
    }

    public void Display20PerHealth()
    {
        _20perHealth.SetActive(true);
    }

    public void Display0PerHealth()
    {
        _0perHealth.SetActive(true);
    }
}
