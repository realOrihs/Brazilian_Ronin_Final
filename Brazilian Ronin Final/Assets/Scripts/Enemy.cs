using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using Invector.vCharacterController;

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
    //public AudioSource soundHit;
    //public AudioSource deadHit;
    //public AudioSource firstPhrase;
    public bool isFirstPrase = true;

    public delegate void OnDamageMake(int Damage);
    public static event OnDamageMake MakeDamage;

    public bool isDead = false;
    public bool isHit = false;
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
                //deadHit.Play();
            }

            if (distance > triggerRadius || !player.isAlive)
            {
                nav.enabled = false;
                anim.SetTrigger("Idle");
                isFirstPrase = true;
                //player.isAnswer = true;
            }
            else
            {
                if (isFirstPrase)
                {
                    //firstPhrase.Play();
                    isFirstPrase = false;
                    Invoke("PlauerAnswer", 1f);
                    //player.isAnswer = false;

                }
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
        Quaternion lookRotation = Quaternion.LookRotation(new Vector3(direction.x, direction.y, direction.z));
        transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * 5f);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "PlayerAttack" && !isDead)
        {
            //soundHit.Play();
            Debug.Log("Get Damage");
            healthPoints -= 1;
            anim.SetTrigger("Hit");
        }
    }

    private void Attack()
    {
        anim.SetTrigger("Idle");
        anim.SetTrigger("Attack");
        //attackTrigger.SetActive(true);
        //Invoke("OffAttack", 1.5f);
    }

    public void OffAttack()
    {
        attackTrigger.SetActive(false);
        if (isHit)
        {
            anim.SetTrigger("IsHit");
            //player.soundHit.Play();
            MakeDamage?.Invoke(1);
            if(HPManager.HPCount != 1)
                //player.soundGetDamage.Play();
            isHit = false;
        }
    }

    //void PlauerAnswer()
    //{
    //    if(player.isAnswer)
    //        player.answerPhrase.Play();
    //}

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, triggerRadius);
    }
}

