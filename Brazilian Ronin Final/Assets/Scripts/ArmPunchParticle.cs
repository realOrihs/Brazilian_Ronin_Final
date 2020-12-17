using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArmPunchParticle : MonoBehaviour
{
    public GameObject punchPartical;
    public CameraShake cameraShake;

    void Start()
    {
        cameraShake = FindObjectOfType<CameraShake>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Enemy")
        {
            //StartCoroutine(Blink());
            Blinking();
            cameraShake.Shake();
        }
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
