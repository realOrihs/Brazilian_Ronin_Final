using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerCamera : MonoBehaviour
{
    public GameObject player;
    public GameObject tower;
    private Vector3 offset;
    void Start()
    {
        offset = transform.position - player.transform.position;
    }

    void LateUpdate()
    {
        if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.D))
        {
            transform.position = player.transform.position + offset;
        }
        transform.LookAt(tower.transform);
    }
}
//using UnityEngine;
//using System.Collections;

//public class TowerCamera : MonoBehaviour
//{

//	public Transform target;
//	public Vector3 offset;
//	public float sensitivity = 3; // чувствительность мышки
//	public float limit = 80; // ограничение вращения по Y
//	public float zoom = 0.25f; // чувствительность при увеличении, колесиком мышки
//	public float zoomMax = 10; // макс. увеличение
//	public float zoomMin = 3; // мин. увеличение
//	private float X, Y;

//	void Start()
//	{
//		limit = Mathf.Abs(limit);
//		if (limit > 90) limit = 90;
//		offset = new Vector3(offset.x, offset.y, -Mathf.Abs(zoomMax) / 2);
//		transform.position = target.position + offset;
//	}

//	void Update()
//	{
//		if (Input.GetAxis("Mouse ScrollWheel") > 0) offset.z += zoom;
//		else if (Input.GetAxis("Mouse ScrollWheel") < 0) offset.z -= zoom;
//		offset.z = Mathf.Clamp(offset.z, -Mathf.Abs(zoomMax), -Mathf.Abs(zoomMin));

//		X = transform.localEulerAngles.y + Input.GetAxis("Mouse X") * sensitivity;
//		Y += Input.GetAxis("Mouse Y") * sensitivity;
//		Y = Mathf.Clamp(Y, -limit, limit);
//		transform.localEulerAngles = new Vector3(-Y, X, 0);
//		transform.position = transform.localRotation * offset + target.position;
//	}
//}
