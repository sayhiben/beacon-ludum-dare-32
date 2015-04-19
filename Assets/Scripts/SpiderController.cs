using UnityEngine;
using System.Collections;

public class SpiderController : MonoBehaviour {

	public float speed = 5;
	public float fireDelay = 1.0f;
	public float rotateSpeed = 50.0f;
	public float aggroRange = 30.0f;
	
	public Transform shotSpawn;
	public Camera mainCamera;
	public GameObject spiderShot;

	private float nextFire;
	private GameObject player;

	void Start(){
		player = GameObject.Find ("Player");
	}

	void Update(){
		transform.localEulerAngles = new Vector3 (30, mainCamera.transform.eulerAngles.y - 180, 0);

		if(InRange() && Time.time >= nextFire){
			nextFire = Time.time + fireDelay;
			GameObject shot = (GameObject)Instantiate (spiderShot, shotSpawn.position, Quaternion.identity);
			shot.transform.LookAt(player.transform);
		}
	}

	bool InRange() {
		return Vector3.Distance(player.transform.position, transform.position) <= aggroRange;
	}
}
