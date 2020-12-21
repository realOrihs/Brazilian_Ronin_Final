using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using Invector.vCharacterController;

public class Enemy : MonoBehaviour
{
    public GameObject playerBody;
    public Player player;
    public GameObject attackZone;
    public float distance;
    private NavMeshAgent nav;
    public float triggerRadius = 10;
    private int healthPoints = 6;
    private static Animator anim;
    //public AudioSource soundHit;
    //public AudioSource deadHit;
    //public AudioSource firstPhrase;
    public bool isFirstPrase = true;

    

    public bool isDead = false;
    //public bool isHit = false;
    void Start()
    {
        nav = GetComponent<NavMeshAgent>();
        anim = gameObject.GetComponent<Animator>();
        player = playerBody.GetComponent<Player>();

        PlayerAttack.MakeDamage += TakeDamage;
    }

    
    // Update is called once per frame
    void Update()
    {
        if (!isDead)
        {
            distance = Vector3.Distance(player.transform.position, transform.position);

            if (healthPoints < 1)
            {
                isDead = true;
                anim.SetBool("IsDead", isDead);
                gameObject.GetComponent<CharacterController>().enabled = false;
                //deadHit.Play();
            }

            if (distance > triggerRadius || !player.isAlive)
            {
                nav.enabled = false;
                anim.SetTrigger("Idle");
                //isFirstPrase = true;
                //player.isAnswer = true;
            }
            else
            {
                if (isFirstPrase)
                {
                    //firstPhrase.Play();
                    //isFirstPrase = false;
                    //Invoke("PlauerAnswer", 1f);
                    //player.isAnswer = false;

                }
                if (distance < nav.stoppingDistance)
                {
                    LookAtPlayer();
                    Attack();
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
        Quaternion lookRotation = Quaternion.LookRotation(new Vector3(direction.x, direction.y, direction.z));
        transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * 5f);
    }

    private void TakeDamage(int num)
    {
        //soundHit.Play();
        //Debug.Log("Get Damage");
        healthPoints -= num;
        anim.SetTrigger("Hit");
    }

    private void Attack()
    {
        //anim.SetTrigger("Idle");
        anim.SetTrigger("Attack");
    }

    //void PlauerAnswer()
    //{
    //    if(player.isAnswer)
    //        player.answerPhrase.Play();
    //}

   // private void OnDrawGizmos()
   //{
   //     Gizmos.color = Color.red;
   //     Gizmos.DrawWireSphere(transform.position, triggerRadius);
   // }
}

