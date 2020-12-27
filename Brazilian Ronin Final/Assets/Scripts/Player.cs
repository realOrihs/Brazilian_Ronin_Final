using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Invector.vCharacterController;
using UnityEngine.Rendering.Universal;
using UnityEngine.Rendering;

public class Player : MonoBehaviour
{
    public bool isAlive { get; private set; }
    private static bool IsGlide = false;
    private static bool isRoll = false;
    private static bool isCollided;
    private static Animator playerAnim;
    public Volume volume { get; private set; }
    public GameObject Menu;
    public GameObject GameOverMenu;
    public AudioSource soundGetDamage;
    
    //public AudioSource soundDead;
    //public AudioSource answerPhrase;
    //public bool isAnswer = true;

    public delegate void OnCoinTake(int num, GameObject coin);
    public static event OnCoinTake TakeCoin;
    public static Player singleton;
    public delegate void PushLever();
    public static event PushLever Push;

    private static Rigidbody playerBody;
    private static vThirdPersonMotor playerMotor;
    private static vThirdPersonController playerController;

    private void Awake()
    {
        isAlive = true;
        singleton = this;
    }
    void Start()
    {
        playerMotor = GetComponent<vThirdPersonMotor>();
        playerController = GetComponent<vThirdPersonController>();           
        playerAnim = GetComponent<Animator>();
        playerBody = GetComponent<Rigidbody>();
        volume = GameObject.FindGameObjectWithTag("UIVolume").GetComponent<Volume>();
        EnemyAttack.MakeDamage += TakeDamage;
    }

    void Update()
    {
        if(playerMotor.inputMagnitude > 0.7 && !SoundManager.singleton.soundRun.isPlaying)
        {
            SoundManager.singleton.soundRun.Play();
        }
        if(playerMotor.inputMagnitude < 0.7 || isRoll || !playerMotor.isGrounded || playerAnim.GetCurrentAnimatorStateInfo(0).IsTag("Attack"))
        {
            SoundManager.singleton.soundRun.Stop();
        }
        if (isRoll && !SoundManager.singleton.soundRoll.isPlaying )
        {
            SoundManager.singleton.soundRoll.pitch = Random.Range(1f, 1.2f);
            SoundManager.singleton.soundRoll.PlayDelayed(0.12f);
        }
        if (!playerMotor.isGrounded)
        {
            isRoll = false;
            SoundManager.singleton.soundRoll.Stop();
        }
        //MoveUp();
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
            SetRotation(5);
        }
        if (Input.GetKeyUp(KeyCode.Mouse0))
        {
            playerAnim.SetBool("Attack", false);
            playerMotor.freeSpeed.runningSpeed = 3;
            SetRotation(15);
        }
        if (Input.GetKeyDown(KeyCode.Mouse1) && playerMotor.groundDistance > 2f)
        {
            IsGlide = true;
            playerAnim.SetBool("IsGlide", IsGlide);
            SetRotation(3);
            playerBody.drag = 4;
            playerBody.useGravity = false;
        }
        if (Input.GetKeyDown(KeyCode.F))
        {
            Push?.Invoke();
        }
        if (IsGlide && (Input.GetKeyUp(KeyCode.Mouse1) || playerMotor.groundDistance < 2f))
        {
            IsGlide = false;
            playerAnim.SetBool("IsGlide", IsGlide);
            playerMotor.freeSpeed.rotationSpeed = 15;
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
        VolumeManager.singleton.ChangeVignette();
        VolumeManager.singleton.Invoke("ChangeVignetteOff", 0.5f);
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
        isRoll = false;
        SoundManager.singleton.soundRun.Stop();
    }

    private void GameOver()
    {
        Cursor.lockState = CursorLockMode.Confined;
        DepthOfField dof;
        if (volume.profile.TryGet(out dof))
        {
            dof.active = true;
        }
        Vignette vg;
        if (volume.profile.TryGet(out vg))
        {
            vg.active = true;
            vg.color.value = new Color(0.86f, 0.14f, 0.14f);
            vg.intensity.value = 0.5f;
        }
        Time.timeScale = 0;
        GameOverMenu.SetActive(true);
    }

    public void SetRotation(float value)
    {
        playerMotor.freeSpeed.rotationSpeed = value;
    }

    public void OnDestroy()
    {
        EnemyAttack.MakeDamage -= TakeDamage;
    }

    private void MoveUp()
    {
        singleton.GetComponent<Rigidbody>().MovePosition(transform.position + new Vector3(0, 1, 0).normalized * 10f * Time.deltaTime);
    }

    public void SetRoll(int state)
    {
        if (state == 1) isRoll = true;
        else isRoll = false;
    }
}

