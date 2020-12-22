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

    public bool isAlive = true;
    private static bool IsGlide = false;
    private static bool isCollided;
    private static Animator playerAnim;
    public static Volume volume;
    public GameObject Menu;
    public GameObject GameOverMenu;
    //public AudioSource soundGetDamage;
    //public AudioSource soundHit;
    //public AudioSource soundDead;
    //public AudioSource answerPhrase;
    //public bool isAnswer = true;

    public delegate void OnCoinTake(int num, GameObject coin);
    public static event OnCoinTake TakeCoin;
    public static Player player;
    public delegate void PushLever();
    public static event PushLever Push;

    private static Rigidbody playerBody;

    private static vThirdPersonMotor playerMotor;

    private static vThirdPersonController playerController;

    void Start()
    {
        playerMotor = GetComponent<vThirdPersonMotor>();
        playerController = GetComponent<vThirdPersonController>();           
        playerAnim = GetComponent<Animator>();
        playerBody = GetComponent<Rigidbody>();
        volume = GameObject.FindGameObjectWithTag("UIVolume").GetComponent<Volume>();
        player = this;
        
        EnemyAttack.MakeDamage += TakeDamage;
    }

    void Update()
    {
        isCollided = false;
        //Debug.Log(playerMotor.groundDistance);
        if (Input.GetKeyDown(KeyCode.Escape) && isAlive)
        {
            PauseGame();
        }
        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            playerAnim.SetBool("Roll", true);
        }
        if (Input.GetKeyUp(KeyCode.LeftShift))
        {
            playerAnim.SetBool("Roll", false);
        }
        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            playerAnim.SetBool("Attack", true);
            playerMotor.freeSpeed.runningSpeed = 0.5f;
            playerMotor.freeSpeed.rotationSpeed = 5;
        }
        if (Input.GetKeyUp(KeyCode.Mouse0))
        {
            playerAnim.SetBool("Attack", false);
            playerMotor.freeSpeed.runningSpeed = 3;
            playerMotor.freeSpeed.rotationSpeed = 15;
        }
        if (Input.GetKeyDown(KeyCode.Mouse1) && playerMotor.groundDistance > 2f)
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
        if (IsGlide && (Input.GetKeyUp(KeyCode.Mouse1) || playerMotor.groundDistance < 1.5f))
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
            Invoke("GameOver",1.5f);
        }
            
    }
    private void OnTriggerEnter(Collider other)
    {
        if (isCollided) return;
        isCollided = true;
        if (other.tag == "Coin") TakeCoin?.Invoke(1,other.gameObject);
    }

    public void TakeDamage(int num)

    {
        playerAnim.SetTrigger("IsHit");
        GetInstance().ChangeVignette();
    }

    public void ChangeVignette()
    {
        Vignette vg;
        if (volume.profile.TryGet(out vg))
        {
            vg.color.value = new Color(0.86f,0.14f,0.14f);
            vg.intensity.value = 0.5f;
            Invoke("ChangeVignetteOff", 0.5f);
        }
    }

    private void ChangeVignetteOff()
    {
        Vignette vg;
        if (volume.profile.TryGet(out vg))
        {
            vg.color.value = new Color(0, 0, 0);
            vg.intensity.value = 0.25f;
        }
    }

    private void PauseGame()
    {
        Cursor.lockState = CursorLockMode.Confined;
        DepthOfField tmp;
        if (volume.profile.TryGet(out tmp))
        {
            tmp.active = true;
        }
        Time.timeScale = 0;
        Menu.SetActive(true);
    }

    private void GameOver()
    {
        Cursor.lockState = CursorLockMode.Confined;
        DepthOfField tmp;
        if (volume.profile.TryGet(out tmp))
        {
            tmp.active = true;
        }
        Vignette vg;
        if (volume.profile.TryGet(out vg))
        {
            vg.color.value = new Color(0.86f, 0.14f, 0.14f);
            vg.intensity.value = 0.5f;
        }
        Time.timeScale = 0;
        GameOverMenu.SetActive(true);
    }

    public static Player GetInstance()
    {
        return player;
    }
}

