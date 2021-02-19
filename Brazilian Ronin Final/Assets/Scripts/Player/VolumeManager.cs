using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;
using UnityEngine.Rendering;
using UnityEngine.SceneManagement;

public class VolumeManager : MonoBehaviour
{
    public static VolumeManager singleton;
    private Volume volume;
    public bool smoothChangeVignetteOn { get; private set; }
    public bool smoothChangeVignetteOff { get; private set; }
    private float vignetteTargetIntensity;
    public Vignette vignette;
    public DepthOfField dof;

    void Awake()
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

        singleton.smoothChangeVignetteOn = false;
        singleton.smoothChangeVignetteOff = false;
        SceneManager.sceneLoaded += OnSceneLoaded;
        singleton.volume = gameObject.GetComponent<Volume>();
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (scene.buildIndex != 0)
        {
            if (singleton.volume.profile.TryGet(out singleton.vignette))
            {
                singleton.vignette.active = false;
            }
            if (singleton.volume.profile.TryGet(out singleton.dof))
            {
                singleton.dof.active = false;
            }
        }
    }

    public void ChangeVignette(Color color, float intensity)
    {
        singleton.smoothChangeVignetteOn = true;
        singleton.vignette.active = true;
        singleton.vignette.color.value = color;
        singleton.vignetteTargetIntensity = intensity;
    }

    private void Update()
    {
        if (singleton.smoothChangeVignetteOn)
        {
            if (singleton.vignette.intensity.value < singleton.vignetteTargetIntensity)
            {
                singleton.vignette.intensity.value += 3 * Time.deltaTime;
            }
            else
            {
                singleton.smoothChangeVignetteOn = false;
                singleton.smoothChangeVignetteOff = true;
            }
        }
        if (singleton.smoothChangeVignetteOff)
        {
            if (singleton.vignette.intensity.value > 0)
            {
                singleton.vignette.intensity.value -= 2 * Time.deltaTime;
            }
            else
            {
                singleton.vignette.active = false;
                singleton.smoothChangeVignetteOff = false;
            }
        }
    }
}
