using UnityEngine;
using System.Collections;

public class SpiderController : MonoBehaviour {

	public float speed = 5;
	public float fireDelay = 0.5f;
	public float rotateSpeed = 50.0f;
	
	public Transform shotSpawn;
	public Camera mainCamera;

	private float nextFire;

	void Update(){
		transform.localEulerAngles = new Vector3 (30, mainCamera.transform.eulerAngles.y - 180, 0);
	}
}
