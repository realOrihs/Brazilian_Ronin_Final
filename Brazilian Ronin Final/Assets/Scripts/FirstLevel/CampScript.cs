﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CampScript : MonoBehaviour
{
    public GameObject[] enemiesGO { get; private set; }
    private List<Enemy> enemies = new List<Enemy>();
    public delegate void isClear(); 
    public static event isClear OnClearCamp;
    void Start()
    {
        Enemy.OnDead += OnEnemyDead;
        OnClearCamp += OpenGates;
        enemiesGO = GameObject.FindGameObjectsWithTag("Enemy");
        foreach(var enemy in enemiesGO)
        {
            if (enemy.GetComponentInParent<CampScript>())
            {
                enemies.Add(enemy.GetComponent<Enemy>());
            }
        }
    }
    
    private void OnEnemyDead(Enemy enemy)
   {
        if (enemies.Contains(enemy))
        {
            enemies.Remove(enemy);
        }

        if(enemies.Count == 0)
        {
            OnClearCamp?.Invoke();
        }
    }

    private void OpenGates()
    {
        GameObject.Find("Gates").GetComponent<Animation>().Play();
    }

    public void OnDestroy()
    {
        Enemy.OnDead -= OnEnemyDead;
        OnClearCamp -= OpenGates;
    }
}
