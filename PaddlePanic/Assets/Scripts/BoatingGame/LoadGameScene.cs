using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadGameScene : MonoBehaviour
{

    // Update is called once per frame
    void Update()
    {
        SceneManager.LoadScene("GameScene");
        //SceneManager.SetActiveScene(SceneManager.GetSceneByName("GameScene"));
        //SceneManager.UnloadSceneAsync("MainMenu");
    }
}
