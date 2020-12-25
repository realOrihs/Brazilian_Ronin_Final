using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;

public class HPManager : MonoBehaviour
{
    public static int HPCount { get; private set; }
    public static int HPmax = 3;
    public static GameObject[] images;
    public static HPManager singleton { get; private set; }

    private void Awake()
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

    private void Start()
    {
        images = GameObject.FindGameObjectsWithTag("HPImage");
        HPCount = HPmax;
        foreach(var img in images)
        {
            img.GetComponent<Image>().enabled = true;
        }
        SortImage();
        EnemyAttack.MakeDamage += TakeDamage;
    }
    
    public static void TakeDamage(int num)
    {
        for(int i = num; i > 0; i--)
        {
            HPCount--;
            if (HPCount < 0) HPCount = 0;
            images[HPCount].GetComponent<Image>().enabled = false;
        }
    }

    private static void SortImage()
    {
        images.OrderBy(img => -img.transform.position.x);
    }
}
