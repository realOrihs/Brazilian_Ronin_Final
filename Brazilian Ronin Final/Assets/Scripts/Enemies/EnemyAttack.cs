using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class EnemyAttack : MonoBehaviour
{
    public delegate void OnDamageMake(int Damage);
    public static event OnDamageMake MakeDamage;
    private bool isCollided;

    void OnTriggerEnter(Collider other)
    {
        if (isCollided) return;
        isCollided = true;
        if (other.tag == "Player")
        {
            MakeDamage?.Invoke(1);
        }
    }

    void Update()
    {
        isCollided = false;
    }
}

