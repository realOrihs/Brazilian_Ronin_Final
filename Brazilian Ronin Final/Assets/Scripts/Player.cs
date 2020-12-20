using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Invector.vCharacterController;

public class Player : MonoBehaviour
{
    public Rigidbody rig;
    public float healthPoints = 3f;
    public Image hp1;
    public Image hp2;
    public Image hp3;

    public GameObject attackZone1;
    public GameObject attackZone2;

    public bool isAlive = true;
    private bool IsGlide = false;
    private Animator playerAnim;

    public AudioSource soundGetDamage;
    public AudioSource soundHit;
    public AudioSource soundDead;
    public AudioSource answerPhrase;
    public bool isAnswer = true;

    public delegate void OnCoinTake(int num, GameObject coin);
    public static event OnCoinTake TakeCoin;

    private Rigidbody playerBody;

    private vThirdPersonMotor playerMotor;

    private vThirdPersonController playerController;


    void Start()
    {
        playerMotor = GetComponent<vThirdPersonMotor>();
        playerController = GetComponent<vThirdPersonController>();           

        playerBody = GetComponent<Rigidbody>();
        Cursor.visible = false;
        playerAnim = gameObject.GetComponent<Animator>();
    }

    public void SetBoolAnimation()
    {
    }
    void Update()
    {
        //Debug.Log(playerMotor.groundDistance);
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Application.Quit();
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
            playerMotor.freeSpeed.runningSpeed = 1;
        }
        if (Input.GetKeyUp(KeyCode.Mouse0))
        {
            playerAnim.SetBool("Attack", false);
            playerMotor.freeSpeed.runningSpeed = 4;
        }
        if (Input.GetKeyDown(KeyCode.Mouse1) && playerMotor.groundDistance > 2.5f)
        {
            IsGlide = true;
            playerAnim.SetBool("IsGlide", IsGlide);
            playerMotor.freeSpeed.rotationSpeed = 3;
            playerBody.drag = 4;
            playerBody.useGravity = false;
        }
        if (IsGlide && (Input.GetKeyUp(KeyCode.Mouse1) || playerMotor.groundDistance < 0.5f))
        {
            IsGlide = false;
            playerMotor.freeSpeed.rotationSpeed = 15;
            playerAnim.SetBool("IsGlide", IsGlide);
            playerBody.drag = 0;
            playerBody.useGravity = true;
        }
            
        if (healthPoints < 3)
        {
            //hp1.gameObject.SetActive(true);
            //hp2.gameObject.SetActive(true);
            //hp3.gameObject.SetActive(false);
            if (healthPoints < 2)
            {
                //hp1.gameObject.SetActive(true);
                //hp2.gameObject.SetActive(false);
                //hp3.gameObject.SetActive(false);
                if (healthPoints < 1 && isAlive)
                {
                    //hp1.gameObject.SetActive(false);
                    playerAnim.SetBool("IsDead", true);
                    soundDead.Play();
                    isAlive = false;
                }
            }
        }
        else
        {
            //hp1.gameObject.SetActive(true);
            //hp2.gameObject.SetActive(true);
            //hp3.gameObject.SetActive(true);
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Coin") TakeCoin?.Invoke(1,other.gameObject);
    }

    public void Attack()
    {
        attackZone1.SetActive(true);
        attackZone2.SetActive(true);
        Invoke("AttackOff", 0.5f);
    }

    public void AttackOff()
    {
        attackZone1.SetActive(false);
        attackZone2.SetActive(false);
    }

}

