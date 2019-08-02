using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TitleButtons : MonoBehaviour {

    public void StartGame()
    {
        //loads other scene
        SceneManager.LoadScene("Scene1");

    }
    public void QuitGame()
    {
        Application.Quit();
    }
    public void BackToTitle()
    {
        SceneManager.LoadScene("Title");
    }
}
