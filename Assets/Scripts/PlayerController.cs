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
	public float jumpPower = 1500.0f;
	public float jumpLightMultiplier = 1.5f;

	public GameObject playerShot;
	public Transform shotSpawn;
	public Light playerLight;
	public Camera mainCamera; 

	private float nextFire;
	private float energy;
	private bool canMove = true;
	private bool isJumping = false;
	private Rigidbody rigidbody;


	void Start(){
		energy = maxEnergy;
		rigidbody = GetComponent<Rigidbody>();
	}

	void Update(){
		UpdateLighting ();
		HandleFire();
		StayOnTheFloor();
	}

	void HandleFire(){
		if(Input.GetButton("Fire1") && Time.time > nextFire && energy >= fireCost) {
			nextFire = Time.time + fireDelay;
			Instantiate(playerShot, shotSpawn.position, transform.rotation);
			energy -= fireCost;
		}
	}

	void FixedUpdate(){
		float moveX = Input.GetAxis("Horizontal");
		float moveY = Input.GetAxis("Vertical");

		HandleJump ();

		if(canMove) {
			Vector3	movement = new Vector3(moveX, 0.0f, moveY);
			rigidbody.AddRelativeForce(movement * speed * Time.deltaTime);
		} else {
			rigidbody.velocity = Vector3.zero;
		}

		transform.Rotate(Vector3.up, rotateSpeed * moveX * Time.deltaTime);
	}
	
	void HandleJump(){
		if(Input.GetKey(KeyCode.Space) && !isJumping && energy >= jumpCost && canMove){
			rigidbody.useGravity = true;
			canMove = false;
			isJumping = true;
			energy -= jumpCost;
			rigidbody.AddRelativeForce(Vector3.up * jumpPower);
			BurstLight();
		}
		if(isJumping && rigidbody.velocity.y <= 0){
			Physics.gravity = Vector3.down * 1500.0f;
		}
	}
	
	void BurstLight(){
		playerLight.range = maxSpotAngle * jumpLightMultiplier;
	}
	
	void StayOnTheFloor(){
		if(transform.position.y <= 1){
			if(isJumping){
				isJumping = false;
				canMove = true;
				rigidbody.useGravity = false;
				Physics.gravity = Vector3.down * -9.81f;
			}
			transform.position = new Vector3(transform.position.x, 1.0f, transform.position.z);
		}
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
		if(!isJumping){
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
}
