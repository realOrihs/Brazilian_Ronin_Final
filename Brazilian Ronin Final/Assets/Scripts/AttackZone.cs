using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Invector.vCharacterController
{
    public class AttackZone : MonoBehaviour
    {
        public Enemy enemy;
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }
        void OnTriggerEnter(Collider other)
        {
            //gameObject.SetActive(false);
            if(other.tag == "Player")
                enemy.isHit = true;
        }

        public void Off()
        {
            //gameObject.SetActive(false);
        }
    }
}
