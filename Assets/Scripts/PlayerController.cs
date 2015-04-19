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
	public float jumpCost = 30.0f;
	public float jumpPower = 20.0f;

	public GameObject playerShot;
	public Transform shotSpawn;
	public Light playerLight;
	public Camera mainCamera;

	private float nextFire;
	private float energy;
	private bool canMove = true;
	private bool isJumping = false;

	void Start(){
		energy = maxEnergy;
	}

	void Update(){
		UpdateLighting ();

		if(transform.position.y <= 1){
			if(isJumping){
				isJumping = false;
				canMove = true;
			}
			transform.position = new Vector3(transform.position.x, 1.0f, transform.position.z);
		}

		if(Input.GetButton("Fire1") && Time.time > nextFire) {
			nextFire = Time.time + fireDelay;
			Instantiate(playerShot, shotSpawn.position, transform.rotation);
			energy -= fireCost;
		}

		if(Input.GetKey(KeyCode.Space) && canMove){
			canMove = false;
			isJumping = true;
			energy -= jumpCost;
			GetComponent<Rigidbody>().AddForce(Vector3.up * jumpPower);
		}
	}

	void FixedUpdate(){
		Rigidbody rigidbody = GetComponent<Rigidbody>();
		float moveX = Input.GetAxis("Horizontal");
		float moveY = Input.GetAxis("Vertical");

		if(canMove) {
			Vector3	movement = new Vector3(moveX, 0.0f, moveY);
			rigidbody.AddRelativeForce(movement * speed * Time.deltaTime);
		} else {
			rigidbody.velocity = Vector3.zero;
		}

		transform.Rotate(Vector3.up, rotateSpeed * moveX * Time.deltaTime);
	}

	public void DisableMovement(){
		canMove = false;
	}

	public void EnableMovement(){
		canMove = true;
	}

	public bool CanMove(){
		return canMove;
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
