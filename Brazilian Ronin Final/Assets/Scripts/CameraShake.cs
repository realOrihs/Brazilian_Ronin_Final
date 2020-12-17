using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraShake : MonoBehaviour
{
    private bool isShake;
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void Shake()
    {
        StartCoroutine(Shaking());
    }
    public IEnumerator Shaking()
    {
        transform.Rotate(new Vector3(0, 0, 90) * Time.deltaTime);
        yield return new WaitForSeconds(10);
    }
}