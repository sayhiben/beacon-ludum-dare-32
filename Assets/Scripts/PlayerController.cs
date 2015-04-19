using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour {

	public float speed = 5;
	public float maxEnergy = 100;
	public float maxSpotAngle = 55.0f;
	public float minSpotAngle = 15.0f;
	public float maxIntensity = 4.0f;
	public float minIntensity = 2.0f;
	public float fireDelay = 0.5f;
	public float fireCost = 5.0f;
	public float rotateSpeed = 50.0f;

	public GameObject playerShot;
	public Transform shotSpawn;
	public Light playerLight;
	public Camera mainCamera;

	private float nextFire;
	private float energy;
	private bool inWeb = false;

	void Start(){
		energy = maxEnergy;
	}

	void Update(){
		UpdateLighting ();

		if(Input.GetButton("Fire1") && Time.time > nextFire) {
			nextFire = Time.time + fireDelay;
			Instantiate(playerShot, shotSpawn.position, transform.rotation);
			energy -= fireCost;
		}
	}

	void FixedUpdate(){
		Rigidbody rigidbody = GetComponent<Rigidbody>();
		float moveX = Input.GetAxis("Horizontal");
		float moveY = Input.GetAxis("Vertical");

		if(!inWeb) {
			Vector3	movement = new Vector3(moveX, 0.0f, moveY);
			rigidbody.AddRelativeForce(movement * speed * Time.deltaTime);
		} else {
			rigidbody.velocity = Vector3.zero;
		}

		transform.Rotate(Vector3.up, rotateSpeed * moveX * Time.deltaTime);
	}


	public void WebSnare(){
		inWeb = true;
	}

	public bool InWeb(){
		return inWeb;
	}

	private void UpdateLighting(){
		playerLight.intensity = Mathf.Clamp(
			(energy / maxEnergy) * maxIntensity,
			minIntensity,
			maxIntensity
		);
		playerLight.range = Mathf.Clamp(
			(energy / maxEnergy) * maxSpotAngle,
			minSpotAngle,
			maxSpotAngle
		);
	}
}
