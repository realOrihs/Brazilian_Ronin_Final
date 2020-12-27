using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;
using UnityEngine.Rendering;

public class VolumeManager : MonoBehaviour
{
    private static Volume volume;
    public static VolumeManager singleton;

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
        }
    }

    void Start()
    {
        volume = gameObject.GetComponent<Volume>();
    }

    public void ChangeVignette()
    {
        Vignette vg;
        if (volume.profile.TryGet(out vg))
        {
            vg.active = true;
            vg.color.value = new Color(0.86f, 0.14f, 0.14f);
            vg.intensity.value = 0.5f;
            //Invoke("ChangeVignetteOff", 0.5f);
        }
    }

    public void ChangeVignetteOff()
    {
        Vignette vg;
        if (volume.profile.TryGet(out vg))
        {
            vg.active = false;
            vg.color.value = new Color(0, 0, 0);
            vg.intensity.value = 0.25f;
        }
    }
}
