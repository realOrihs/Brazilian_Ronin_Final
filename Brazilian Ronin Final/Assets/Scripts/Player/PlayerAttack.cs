using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    public GameObject punchPartical;
    public CameraShake cameraShake;

    public delegate void OnDamageMake(int num,Enemy enemy);
    public static event OnDamageMake MakeDamage;
    private bool isCollided;
    
    //public AudioSource audioHit;
    //public AudioSource audioGetDamage;

    void Start()
    {
        cameraShake = FindObjectOfType<CameraShake>();
    }

    void OnTriggerEnter(Collider other)
    {
        if (isCollided) return;
        isCollided = true;
        if (other.tag == "Enemy")
        {
            MakeDamage?.Invoke(1,other.gameObject.GetComponent<Enemy>());
            Blinking();
            cameraShake.Shake();
            SoundManager.singleton.soundHitLeft.pitch = Random.Range(0.85f, 1.15f);
            SoundManager.singleton.soundHitRight.pitch = Random.Range(0.85f, 1.15f);
            SoundManager.singleton.soundHitLeft.Play();
            SoundManager.singleton.soundHitRight.Play();
        }
    }

    void Update()
    {
        isCollided = false;
    }
    public void Blinking()
    {
        punchPartical.SetActive(true);
        Invoke("UnBlink", 0.5f);
    }

    public void UnBlink()
    {
        punchPartical.SetActive(false);
    }

    //public IEnumerator Blink()
    //{
    //    punchPartical.SetActive(true);
    //    yield return new WaitForSeconds(1);
    //    punchPartical.SetActive(false);
    //}
    //void OnTriggerExit(Collider other)
    //{
    //    if (other.tag == "Enemy")
    //    {
    //        punchPartical.SetActive(false);
    //    }
    //}
}
