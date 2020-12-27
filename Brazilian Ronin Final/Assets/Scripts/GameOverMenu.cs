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
        Vignette vg;
        if (volume.profile.TryGet(out vg))
        {
            vg.active = false;
        }
        DepthOfField dof;
        if (volume.profile.TryGet(out dof))
        {
            dof.active = false;
        }
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

    private void Start()
    {
        volume = GameObject.FindGameObjectWithTag("UIVolume").GetComponent<Volume>();
    }
}

