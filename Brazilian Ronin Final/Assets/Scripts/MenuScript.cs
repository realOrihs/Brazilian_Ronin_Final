using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuScript : MonoBehaviour
{
    void Update()
    {
        if(HPManager.singleton != null && HPManager.singleton.gameObject.GetComponent<Canvas>().enabled == true)
        HPManager.singleton.gameObject.GetComponent<Canvas>().enabled = false;
    }

    public void NewGame()
    {
        SceneManager.LoadScene("Main Scene");
    }

    public void Exit()
    {
        Application.Quit();
    }
}
