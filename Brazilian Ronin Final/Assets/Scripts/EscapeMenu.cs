using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using UnityEngine.SceneManagement;

public class EscapeMenu : MonoBehaviour
{
    public GameObject Menu;
    private Volume volume;
    
    public void Continue()
    {
        DepthOfField dof;
        if (volume.profile.TryGet(out dof))
        {
            dof.active = false;
        }
        Menu.SetActive(false);
        Time.timeScale = 1;
        Cursor.lockState = CursorLockMode.Locked;
    }

    public void Retry()
    {
        Time.timeScale = 1;
        SceneManager.LoadSceneAsync("Main Scene");
        DepthOfField dof;
        if (volume.profile.TryGet(out dof))
        {
            dof.active = false;
        }
        Cursor.lockState = CursorLockMode.Locked;
    }

    public void ToMainMenu()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene("Main menu");
        DepthOfField dof;
        if (volume.profile.TryGet(out dof))
        {
            dof.active = false;
        }
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

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            //Continue();
            //Cursor.lockState = CursorLockMode.Locked;
        }
    }
}
