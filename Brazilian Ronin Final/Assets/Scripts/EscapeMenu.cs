using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using UnityEngine.SceneManagement;

public class EscapeMenu : MonoBehaviour
{
    public GameObject Menu;
    public Volume volume;
    // Start is called before the first frame update
    public void Continue()
    {
        DepthOfField tmp;
        if (volume.profile.TryGet(out tmp))
        {
            tmp.focusDistance.value = 3f;
        }
        Menu.SetActive(false);
        Time.timeScale = 1;
        Cursor.lockState = CursorLockMode.Locked;
    }

    public void Retry()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene("Main Scene");
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

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
           // Continue();
        }
    }
}
