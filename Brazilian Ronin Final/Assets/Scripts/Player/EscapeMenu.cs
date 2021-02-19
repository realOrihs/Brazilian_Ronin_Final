using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using UnityEngine.SceneManagement;

public class EscapeMenu : MonoBehaviour
{
    public GameObject Menu { get; private set; }
    
    public void Continue()
    {
        VolumeManager.singleton.dof.active = false;
        Menu.SetActive(false);
        Time.timeScale = 1;
        Cursor.lockState = CursorLockMode.Locked;
    }

    public void Retry()
    {
        //Time.timeScale = 1;
        LevelLoader.singleton.LoadLevel(SceneManager.GetActiveScene().buildIndex);
        Cursor.lockState = CursorLockMode.Locked;
    }

    public void ToMainMenu()
    {
        LevelLoader.singleton.LoadLevel(0);
        VolumeManager.singleton.dof.active = false;
    }

    public void Exit()
    {
        Time.timeScale = 1;
        Application.Quit();
    }

    private void Start()
    {
        Menu = GameObject.Find("Escape Menu");
        SceneManager.sceneLoaded -= OnSceneLoaded;
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        Time.timeScale = 1;
    }
}
