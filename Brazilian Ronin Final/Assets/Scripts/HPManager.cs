using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HPManager : MonoBehaviour
{
    public static int HPCount { get; private set; }
    public static int HPmax = 3;
    public Image hp1;
    public Image hp2;
    public Image hp3;
    public Image[] images;

    private void Start()
    {
        HPCount = HPmax;
        EnemyAttack.MakeDamage += TakeDamage;
        images = new Image[] { hp1,hp2,hp3 };
    }
    private void TakeDamage(int num)
    {
        HPCount -= num;
        if (HPCount < 0) HPCount = 0;
        Debug.Log(HPCount);
        images[HPCount].enabled = false;
    }
}
