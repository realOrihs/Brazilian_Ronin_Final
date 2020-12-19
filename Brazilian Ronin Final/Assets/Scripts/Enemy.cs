using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace Invector.vCharacterController
{
    public class Enemy : MonoBehaviour
    {
        public GameObject playerBody;
        public Player player;
        public GameObject attackTrigger;
        public float distance;
        NavMeshAgent nav;
        public float triggerRadius = 10;
        public float healthPoints = 3f;
        private Animator anim;

        public bool isDead = false;
        void Start()
        {
            nav = GetComponent<NavMeshAgent>();
            anim = gameObject.GetComponent<Animator>();
            player = playerBody.GetComponent<Player>();
        }

        // Update is called once per frame
        void Update()
        {
            if (!isDead)
            {
                attackTrigger.SetActive(false);
                distance = Vector3.Distance(player.transform.position, transform.position);

                if (healthPoints < 1)
                {
                    isDead = true;
                    anim.SetBool("IsDead", isDead);
                }

                if (distance > triggerRadius || !player.isAlive)
                {
                    nav.enabled = false;
                    anim.SetTrigger("Idle");
                }
                else
                {
                    if (distance < nav.stoppingDistance)
                    {
                        LookAtPlayer();
                        if (!attackTrigger.activeInHierarchy)
                        {
                            Attack();
                        }
                    }
                    else
                    {
                        nav.enabled = true;
                        nav.SetDestination(player.transform.position);
                        anim.SetTrigger("Run");
                    }
                }
            }
        }

        void LookAtPlayer()
        {
            Vector3 direction = (player.transform.position - transform.position).normalized;
            Quaternion lookRotation = Quaternion.LookRotation(new Vector3(direction.x, 0, direction.z));
            transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * 5f);
        }

        void OnTriggerEnter(Collider other)
        {
            if (other.tag == "PlayerAttack" && !isDead)
            {
                Debug.Log("Get Damage");
                healthPoints -= 1;
                anim.SetTrigger("Hit");
            }
        }

        private void Attack()
        {
            anim.SetTrigger("Idle");
            anim.SetTrigger("Attack");
            attackTrigger.SetActive(true);
            Invoke("OffAttack", 1.5f);
        }

        public void OffAttack()
        {
            attackTrigger.SetActive(false);
        }

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position, triggerRadius);
        }
    }
}
