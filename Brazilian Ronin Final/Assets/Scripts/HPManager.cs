using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;

public class HPManager : MonoBehaviour
{
    public static int HPCount { get; private set; }
    public static int HPmax = 3;
    public static GameObject[] imagesGO;
    public static IEnumerable<Image> images;
    public static HPManager singleton;

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

        if (imagesGO == null)
        {
            imagesGO = GameObject.FindGameObjectsWithTag("HPImage");
        }

        HPCount = HPmax;
        SortImage();
        foreach (var img in images)
        {
            img.enabled = true;
        }
       
        singleton.gameObject.GetComponent<Canvas>().worldCamera = GameObject.Find("UI camera").GetComponent<Camera>();
    }

    private void Start()
    {
        //EnemyAttack.MakeDamage += TakeDamage;
    }
    
    public static void TakeDamage(int num)
    {
        for(int i = num; i > 0; i--)
        {
            HPCount--;
            if (HPCount < 0) HPCount = 0;
            images.ToArray()[HPCount].enabled = false;
        }
    }

    private void SortImage()
    {
        images = imagesGO.OrderBy(img => img.transform.position.x).Select(img => img.GetComponent<Image>());
    }

    private void OnDestroy()
    {
        //EnemyAttack.MakeDamage -= TakeDamage;
    }
}
