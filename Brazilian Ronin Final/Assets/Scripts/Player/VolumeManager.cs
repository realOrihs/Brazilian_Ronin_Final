using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;
using UnityEngine.Rendering;
using UnityEngine.SceneManagement;

public class VolumeManager : MonoBehaviour
{
    public static VolumeManager singleton;
    private static Volume volume;
    public static bool smoothChangeVignetteOn { get; private set; }
    public static bool smoothChangeVignetteOff { get; private set; }
    private static float vignetteTargetIntensity;
    public static Vignette vignette;
    public static DepthOfField dof;

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

        smoothChangeVignetteOn = false;
        smoothChangeVignetteOff = false;
        SceneManager.sceneLoaded += OnSceneLoaded;
        volume = gameObject.GetComponent<Volume>();
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (scene.buildIndex != 0)
        {
            if (volume.profile.TryGet(out vignette))
            {
                vignette.active = false;
            }
            if (volume.profile.TryGet(out dof))
            {
                dof.active = false;
            }
        }
    }

    public void ChangeVignette(Color color, float intensity)
    {
        smoothChangeVignetteOn = true;
        vignette.active = true;
        vignette.color.value = color;
        vignetteTargetIntensity = intensity;
    }

    private void Update()
    {
        if (smoothChangeVignetteOn)
        {
            if (vignette.intensity.value < vignetteTargetIntensity)
            {
                vignette.intensity.value += 3 * Time.deltaTime;
            }
            else
            {
                smoothChangeVignetteOn = false;
                smoothChangeVignetteOff = true;
            }
        }
        if (smoothChangeVignetteOff)
        {
            if (vignette.intensity.value > 0)
            {
                vignette.intensity.value -= 2 * Time.deltaTime;
            }
            else
            {
                vignette.active = false;
                smoothChangeVignetteOff = false;
            }
        }
    }
}
