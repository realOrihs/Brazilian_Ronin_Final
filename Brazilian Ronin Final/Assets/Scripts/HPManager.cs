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
    public static GameObject[] images;

    private void Start()
    {
        images = GameObject.FindGameObjectsWithTag("HPImage");
        HPCount = HPmax;
        EnemyAttack.MakeDamage += TakeDamage;
     }
    private void TakeDamage(int num)
    {
        HPCount -= num;
        if (HPCount < 0) HPCount = 0;
        images[HPCount].GetComponent<Image>().enabled = false;
    }
}
