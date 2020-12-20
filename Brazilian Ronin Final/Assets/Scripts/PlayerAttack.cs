using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    public GameObject punchPartical;
    public CameraShake cameraShake;

    public delegate void OnDamageMake(int num);
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
            //StartCoroutine(Blink());
            //audioHit.pitch = Random.Range(0.8f, 1.2f);
            //audioHit.Play();
            // audioGetDamage.Play();
            MakeDamage?.Invoke(1);
            Blinking();
            cameraShake.Shake();
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
