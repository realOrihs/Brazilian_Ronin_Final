using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UICamera : MonoBehaviour
{
    public static UICamera singleton;
    void Awake()
    {
        //if (!singleton)
        //{
        //    singleton = this;
        //    DontDestroyOnLoad(this);

        //}
        //else
        //{
        //    Destroy(gameObject);
        //}
    }
}
