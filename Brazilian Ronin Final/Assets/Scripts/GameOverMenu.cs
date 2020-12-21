using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using UnityEngine.SceneManagement;

public class GameOverMenu : MonoBehaviour
{
    public Volume volume;

    public void Retry()
    {
        Time.timeScale = 1;
        SceneManager.LoadSceneAsync("Main Scene");
        Cursor.lockState = CursorLockMode.Locked;
    }

    public void ToMainMenu()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene("Main menu");
    }

    public void Exit()
    {
        Time.timeScale = 1;
        Application.Quit();
    }
}

