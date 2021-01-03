using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LevelLoader : MonoBehaviour
{
    public GameObject loadingScreen;
    public Slider slider;
    public static LevelLoader singleton;

    public void Awake()
    {
        if (!singleton)
        {
            singleton = this;
            DontDestroyOnLoad(this);
        }
        else
        {
            Destroy(gameObject);
            return;
        }

        singleton.loadingScreen = loadingScreen;
        singleton.slider = slider;
    }

    public void LoadLevel(int sceneIndex)
    {
        StartCoroutine(LoadAsync(sceneIndex));
        singleton.loadingScreen.SetActive(true);
    }

    IEnumerator LoadAsync(int sceneIndex)
    {
        AsyncOperation operation = SceneManager.LoadSceneAsync(sceneIndex);

        while (!operation.isDone)
        {
            float progress = Mathf.Clamp01(operation.progress / 0.9f);
            singleton.slider.value = progress;
            yield return null;
        }

        if (operation.isDone) singleton.Invoke(nameof(OffLoadingScreen),0.2f);
    }

    private void OffLoadingScreen()
    {
        singleton.loadingScreen.SetActive(false);
    }
}
