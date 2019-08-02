using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PauseManager : MonoBehaviour
{

    static PauseManager _instance = null;

    public CanvasGroup cg;
    public static bool isPaused;

    Character c;

    void Awake()
    {
        c = GameObject.FindGameObjectWithTag("Player").GetComponent<Character>();
    }
    // Use this for initialization
    void Start()
    {
        cg.alpha = 0.0f;
    }

    // Update is called once per frame
    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            isPaused = !isPaused;
            PauseGame();
        }
    }
    void PauseGame()
    {
        if (cg.alpha == 0.0f)
        {
            cg.alpha = 1.0f;
            Time.timeScale = 0;
        }
        else
        {
            cg.alpha = 0.0f;
            Time.timeScale = 1;
        }

    }
    public void BackToTitle()
    {
        if (isPaused)
        {
            cg.alpha = 0.0f;
            Time.timeScale = 1;
            SceneManager.LoadScene("Title");
        }

    }
    
    public void Resume()
    {
        if (isPaused)
        {
            if (cg.alpha == 0.0f)
            {
                cg.alpha = 1.0f;
                Time.timeScale = 0;
            }
            else
            {
                cg.alpha = 0.0f;
                Time.timeScale = 1;
            }
            isPaused = false;
        }
    }
    public static PauseManager instance
    {
        get { return _instance; }
    }
}

