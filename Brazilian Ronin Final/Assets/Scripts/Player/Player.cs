using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Invector.vCharacterController;
using UnityEngine.Rendering.Universal;
using UnityEngine.Rendering;

public class Player : MonoBehaviour
{
    public bool isAlive { get; private set; }
    public bool isGlide { get; private set; }
    public bool isRoll { get; private set; }
    public bool isCollided { get; private set; }
    public bool isGetFallDamage { get; private set; }
    public static Animator playerAnim { get; private set; }
    public Volume volume { get; private set; }
    public GameObject Menu;
    public GameObject GameOverMenu;

    public delegate void OnCoinTake(int num, GameObject coin);
    public static event OnCoinTake TakeCoin;
    public delegate void OnDead();
    public static event OnDead isDead;
    public static Player singleton;
    public delegate void PushLever();
    public static event PushLever Push;
    public delegate void OnFallDamageMake(int num);
    public static event OnFallDamageMake MakeFallDamage;

    private static Rigidbody playerBody;
    private static vThirdPersonMotor playerMotor;

    private void Awake()
    {
        isAlive = true;
        singleton = this;
    }
    void Start()
    {
        isRoll = false;
        isGlide = false;
        playerMotor = GetComponent<vThirdPersonMotor>();
        playerAnim = GetComponent<Animator>();
        playerBody = GetComponent<Rigidbody>();
        volume = GameObject.FindGameObjectWithTag("UIVolume")?.GetComponent<Volume>();
        EnemyAttack.MakeDamage += TakeDamage;
        Player.MakeFallDamage += TakeDamage;
    }

    void Update()
    {
        isCollided = false;
        if(playerMotor.inputMagnitude > 0.7 && SoundManager.singleton?.soundRun.isPlaying == false)
        {
            SoundManager.singleton?.soundRun.Play();
        }
        if(playerMotor.inputMagnitude < 0.7 || isRoll || !playerMotor.isGrounded || playerAnim.GetCurrentAnimatorStateInfo(0).IsTag("Attack"))
        {
            SoundManager.singleton?.soundRun.Stop();
        }
        if (isRoll && SoundManager.singleton?.soundRoll.isPlaying == false)
        {
            if(SoundManager.singleton) SoundManager.singleton.soundRoll.pitch = Random.Range(1f, 1.2f);
            SoundManager.singleton?.soundRoll.PlayDelayed(0.12f);
        }
        if (!playerMotor.isGrounded)
        {
            isRoll = false;
            SoundManager.singleton?.soundRoll.Stop();
        }
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
            //CancelInvoke(nameof(SetAttackFalse));
        }
        if (playerAnim.GetCurrentAnimatorStateInfo(0).IsTag("Attack"))
        {
            playerMotor.freeSpeed.runningSpeed = 0.5f;
            SetRotation(5f);
        }
        if (Input.GetKeyUp(KeyCode.Mouse0))
        {
            //Invoke(nameof(SetAttackFalse), 0.29f);
            playerAnim.SetBool("Attack", false);
        }
        if (!playerAnim.GetCurrentAnimatorStateInfo(0).IsTag("Attack"))
        {
            playerMotor.freeSpeed.runningSpeed = 3;
            SetRotation(15);
        }
        if (Input.GetKeyDown(KeyCode.Mouse1) && playerMotor.groundDistance > 2f)
        {
            isGlide = true;
            playerAnim.SetBool("IsGlide", isGlide);
            SetRotation(3);
            playerBody.drag = 4;
            playerBody.useGravity = false;
        }
        if (Input.GetKeyDown(KeyCode.F))
        {
            Push?.Invoke();
        }
        if (isGlide && (Input.GetKeyUp(KeyCode.Mouse1) || playerMotor.groundDistance < 2f))
        {
            isGlide = false;
            playerAnim.SetBool("IsGlide", isGlide);
            playerMotor.freeSpeed.rotationSpeed = 15;
            playerBody.drag = 0;
            playerBody.useGravity = true;
        }
        if (HPManager.singleton?.HPCount < 1 && isAlive)
        {
            playerAnim.SetBool("IsDead", true);
            playerMotor.enabled = false;
            isAlive = false;
            Invoke("GameOver",1.5f);
            isDead?.Invoke();
        }
        //if (!isGetFallDamage && playerMotor.groundDistance > playerMotor.distanceToDamage)
        //    isGetFallDamage = true;
        //if (isGetFallDamage && playerMotor.isGrounded)
        //{
        //    MakeFallDamage.Invoke(1);
        //    isGetFallDamage = false;
        //}
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
    }

    private void PauseGame()
    {
        Cursor.lockState = CursorLockMode.Confined;
        if(VolumeManager.singleton) VolumeManager.singleton.dof.active = true;
        Time.timeScale = 0;
        Menu?.SetActive(true);
        isRoll = false;
        SoundManager.singleton?.soundRun.Stop();
    }

    private void GameOver()
    {
        Cursor.lockState = CursorLockMode.Confined;
        if (VolumeManager.singleton)
        {
            VolumeManager.singleton.dof.active = true;
            VolumeManager.singleton.vignette.active = true;
            VolumeManager.singleton.vignette.color.value = new Color(0.86f, 0.14f, 0.14f);
            VolumeManager.singleton.vignette.intensity.value = 0.5f;
        }
        Time.timeScale = 0;
        GameOverMenu?.SetActive(true);
    }

    public void SetRotation(float value)
    {
        playerMotor.freeSpeed.rotationSpeed = value;
    }

    public void OnDestroy()
    {
        EnemyAttack.MakeDamage -= TakeDamage;
    }

    public void SetRoll(int state)
    {
        if (state == 1) isRoll = true;
        else isRoll = false;
    }

    public void SetAttackFalse() //see invoke of this 
    {
        //playerAnim.SetBool("Attack", false);
    }
}

