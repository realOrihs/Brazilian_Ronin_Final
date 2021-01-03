using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using Invector.vCharacterController;

public class Enemy : MonoBehaviour
{
    private Player player;
    private float distance;
    private NavMeshAgent nav;
    private float triggerRadius;
    private int healthPoints;
    private Animator anim;
    //public AudioSource soundHit;
    //public AudioSource deadHit;
    //public AudioSource firstPhrase;
    private bool isFirstPrase = true;
    public delegate void OnDeadEv(Enemy enemy);
    public static event OnDeadEv OnDead;

    private bool isDead = false;
    //public bool isHit = false;
    void Start()
    {
        nav = GetComponent<NavMeshAgent>();
        anim = gameObject.GetComponent<Animator>();
        player = Player.singleton;
        healthPoints = 5;
        triggerRadius = 13;
        PlayerAttack.MakeDamage += TakeDamage;
    }

    public Enemy GetInstance()
    {
        return this;
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
                OnDead?.Invoke(this);
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
                    anim.SetBool("Attack", false);
                    nav.enabled = true;
                    nav.SetDestination(player.transform.position);
                    anim.SetTrigger("Run");
                    if(SoundManager.currentClip != SoundManager.singleton.battleMusic)
                    SoundManager.PlayMusic(SoundManager.singleton.battleMusic, 0.03f, true);
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

    private void TakeDamage(int num,Enemy enemy)
    {
        if (this == enemy)
        {
            //soundHit.Play();
            //Debug.Log("Get Damage");
            healthPoints -= num;
            anim.SetTrigger("Hit");
        }
    }

    private void Attack()
    {
        //anim.SetTrigger("Idle");
        anim.SetBool("Attack", true);
        //SoundManager.singleton.soundAxe.Play();
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

