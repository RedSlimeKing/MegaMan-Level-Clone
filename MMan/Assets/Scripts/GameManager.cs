using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour {

    static LoadSaveManager stateManager = null;
    static GameManager instance = null;
    
    public Button ButtonBack;
    public Button ButtonQuit;

    public static GameManager Instance
    {
        get
        {
            if (!instance)
                instance = new GameObject("GameManager").AddComponent<GameManager>();
            return instance;
        }
    }
    public static LoadSaveManager StateManager
    {
        get
        {
            if (!stateManager)
            {
                stateManager = instance.GetComponent<LoadSaveManager>();
            }
            return stateManager;
        }
    }
    public void update()
    {
        ButtonBack = GameObject.Find("Back To Title").GetComponent<Button>();
        ButtonQuit = GameObject.Find("Quit Button").GetComponent<Button>();

        ButtonBack.onClick.AddListener(delegate () { BackToTitle(); });
        ButtonQuit.onClick.AddListener(delegate () { QuitGame(); });
    }
    void Awake()
    {
        if (instance && instance.GetInstanceID() != GetInstanceID())
        {
            Destroy(gameObject);
        }
        else
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
    }
    public void SaveGame()
    {
        Debug.Log(Application.persistentDataPath);
        stateManager.Save(Application.persistentDataPath + "/SaveGame.xml");

    }
    public void LoadGame()
    {
        stateManager.Load(Application.persistentDataPath + "/SaveGame.xml");
    }

    public void StartGame()
    {
        //loads other scene
        SceneManager.LoadScene("Scene1");

    }
    
    public void QuitGame()
    {
        Application.Quit();
    }

    public void Credits()
    {
        SceneManager.LoadScene("Credits");
    }
    public void GameOver()
    {
        SceneManager.LoadScene("GameOver");
    }
    public void BackToTitle()
    {
        SceneManager.LoadScene("Title");
    }
}
