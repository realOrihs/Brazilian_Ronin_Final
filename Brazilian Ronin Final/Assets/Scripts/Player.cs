using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Invector.vCharacterController;
using UnityEngine.Rendering.Universal;
using UnityEngine.Rendering;

public class Player : MonoBehaviour
{
    public Rigidbody rig;

    public GameObject attackZone1;
    public GameObject attackZone2;

    public bool isAlive = true;
    private bool IsGlide = false;
    private Animator playerAnim;
    public Volume volume;
    public GameObject Menu;
    //public AudioSource soundGetDamage;
    //public AudioSource soundHit;
    //public AudioSource soundDead;
    //public AudioSource answerPhrase;
    //public bool isAnswer = true;

    public delegate void OnCoinTake(int num, GameObject coin);
    public static event OnCoinTake TakeCoin;

    public delegate void PushLever();
    public static event PushLever Push;

    private Rigidbody playerBody;

    private vThirdPersonMotor playerMotor;

    private vThirdPersonController playerController;

    void Start()
    {
        playerMotor = GetComponent<vThirdPersonMotor>();
        playerController = GetComponent<vThirdPersonController>();           
        playerAnim = GetComponent<Animator>();
        playerBody = GetComponent<Rigidbody>();
        EnemyAttack.MakeDamage += TakeDamage;
        
    }

    void Update()
    {
        //Debug.Log(playerMotor.groundDistance);
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            PauseGame();
        }
        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            playerAnim.SetBool("Roll", true);
            //rig.AddForce(moveDirection * 10, ForceMode.VelocityChange);
        }
        if (Input.GetKeyUp(KeyCode.LeftShift))
        {
            playerAnim.SetBool("Roll", false);
            //rig.AddForce(moveDirection * 10, ForceMode.VelocityChange);
        }
        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            playerAnim.SetBool("Attack", true);
            playerMotor.freeSpeed.runningSpeed = 0.5f;
            Attack();
        }
        if (Input.GetKeyUp(KeyCode.Mouse0))
        {
            playerAnim.SetBool("Attack", false);
            playerMotor.freeSpeed.runningSpeed = 3;
        }
        if (Input.GetKeyDown(KeyCode.Mouse1) && playerMotor.groundDistance > 2.5f)
        {
            IsGlide = true;
            playerAnim.SetBool("IsGlide", IsGlide);
            playerMotor.freeSpeed.rotationSpeed = 3;
            playerBody.drag = 4;
            playerBody.useGravity = false;
        }
        if (Input.GetKeyDown(KeyCode.F))
        {
            Push?.Invoke();
        }
        if (IsGlide && (Input.GetKeyUp(KeyCode.Mouse1) || playerMotor.groundDistance < 0.5f))
        {
            IsGlide = false;
            playerMotor.freeSpeed.rotationSpeed = 15;
            playerAnim.SetBool("IsGlide", IsGlide);
            playerBody.drag = 0;
            playerBody.useGravity = true;
        }
        if (HPManager.HPCount < 1 && isAlive)
        {
            playerAnim.SetBool("IsDead", true);
            // soundDead.Play();
            isAlive = false;
        }
            
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Coin") TakeCoin?.Invoke(1,other.gameObject);
    }

    public void Attack()
    {
        attackZone1.SetActive(true);
        //attackZone2.SetActive(true);
        Invoke("AttackOff", 0.5f);
    }

    public void AttackOff()
    {
        attackZone1.SetActive(false);
        attackZone2.SetActive(false);
    }

    public void TakeDamage(int num)
    {
        playerAnim.SetTrigger("IsHit");
    }

    private void PauseGame()
    {
        Cursor.lockState = CursorLockMode.Confined;
        DepthOfField tmp;
        if (volume.profile.TryGet(out tmp))
        {
            tmp.focusDistance.value = 1f;
        }
        Time.timeScale = 0;
        Menu.SetActive(true);
    }
}

