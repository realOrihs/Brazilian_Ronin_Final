using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArmPunchParticle : MonoBehaviour
{
    public ParticleSystem rightPunch;

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnTriggerEnter(Collider other)
    {
        //if (other.tag == "Enemy")
        //{
        //rightPunch.enableEmission = !rightPunch.enableEmission;
        //}
    }

}
