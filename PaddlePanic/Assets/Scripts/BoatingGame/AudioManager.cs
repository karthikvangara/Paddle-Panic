using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance;
    public GameObject wavesEffect;
    public GameObject hittingWoddenLogsEffect;
    public string gameScene;

    public void Awake()
    {
        instance = this;
    }

    public void Update()
    {
        if (SceneManager.GetActiveScene().name == gameScene)
        {
            EnableWavesEffect();
        }
        else
        {
            DisableWavesEffect();
        }
    }

    public void EnableWavesEffect()
    {
        wavesEffect.SetActive(true);
    }

    public void DisableWavesEffect()
    {
        wavesEffect.SetActive(false);
    }

    public void EnableHittingWoddenLogsEffect()
    {
        hittingWoddenLogsEffect.SetActive(true);
    }

    public void DisableHittingWoddenLogsEffect()
    {
        hittingWoddenLogsEffect.SetActive(false);
    }
}
