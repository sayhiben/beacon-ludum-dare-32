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
	public float animationSpeed = 0.25f;
	public float turnMultiplier = 0.25f;

	public GameObject playerShot;
	public Transform shotSpawn;
	public Light playerLight;
	public Camera mainCamera;
	public GameObject wingsBillboard;
	public GameObject bodyBillboard;
	public Texture[] wingFrames;
	public Texture[] bodyFrames;
	public AudioSource shootSound;
	public AudioSource healSound;
	public AudioSource webSnareSound;
	public AudioSource hurtSound;

	private float nextFire;
	private float nextAnimationTime;
	private float energy;
	private bool isJumping = false;
	private Rigidbody rbody;
	private GameObject webSnare;
	private Vector3 startPosition;
	private int wingFrame = 0;
	private int bodyFrame = 0;
	private GameObject levelManager;

	void Start(){
		energy = maxEnergy;
		startPosition = transform.position;
		rbody = GetComponent<Rigidbody>();
		levelManager = GameObject.Find("Level Manager");
	}

	void Update(){
		if(energy <= 0 && !levelManager.GetComponent<LevelController>().IsTransitioning()){
			levelManager.GetComponent<LevelController>().RestartLevel();
		}
		UpdateLighting ();
		HandleFire();
		UpdatePlayerBody();
		StayOnTheFloor();
	}

	float EnergyPercent(){
		return energy / maxEnergy;
	}

	public void Heal(float healAmount){
		healSound.Play ();
		energy += healAmount;
	}

	void UpdatePlayerBody(){
		int newFrame = (int)Mathf.Ceil(EnergyPercent() / 0.2f);
		newFrame = Mathf.Clamp(newFrame, 0, bodyFrames.Length - 1);
		if(newFrame != bodyFrame){
			bodyFrame = newFrame;
			MeshRenderer renderer = bodyBillboard.GetComponent<MeshRenderer>();
			renderer.material.mainTexture = bodyFrames[bodyFrame];
		}
	}

	void HandleFire(){
		if((Input.GetKeyDown(KeyCode.Space) || Input.GetButton("Fire1")) && Time.time > nextFire && energy >= fireCost) {
			nextFire = Time.time + fireDelay;
			Instantiate(playerShot, shotSpawn.position, transform.rotation);
			energy -= fireCost;
			shootSound.Play();
		}
	}

	void FixedUpdate(){
		float moveX = Input.GetAxis("Horizontal");
		float moveY = Input.GetAxis("Vertical");

		if(CanMove()) {
			Vector3	movement = new Vector3(moveX * turnMultiplier, 0.0f, moveY);
			rbody.AddRelativeForce(movement * speed * Time.deltaTime);
		} else {
			rbody.velocity = Vector3.zero;
		}

		if(rbody.velocity != Vector3.zero){
			if(Time.time >= nextAnimationTime){
				Animate();
				wingFrame++;
				if(wingFrame >= wingFrames.Length){
					wingFrame = 0;
				}
				nextAnimationTime = Time.time + animationSpeed;
			}
			PlayWingSound();
		} else {
			StopWingSound();
			wingFrame = 3; // closed
			Animate ();
		}

		transform.Rotate(Vector3.up, rotateSpeed * moveX * Time.deltaTime);
	}

	void PlayWingSound(){
		AudioSource wingSound = gameObject.GetComponent<AudioSource>();
		if(!wingSound.isPlaying){
			wingSound.Play();
		}
	}

	void StopWingSound(){
		AudioSource wingSound = gameObject.GetComponent<AudioSource>();
		if(wingSound.isPlaying){
			wingSound.Stop();
		}
	}

	void Animate(){
		MeshRenderer renderer = wingsBillboard.GetComponent<MeshRenderer>();
		renderer.material.mainTexture = wingFrames[wingFrame];
	}
	
	void HandleJump(){
		if(Input.GetKey(KeyCode.Space) && CanJump() && energy >= jumpCost){
			rbody.useGravity = true;
			isJumping = true;
			energy -= jumpCost;
			rbody.AddRelativeForce(Vector3.up * jumpPower);
			BurstLight();
		}
		if(isJumping && rbody.velocity.y <= 0){
			Physics.gravity = Vector3.down * 1500.0f;
		}
	}

	bool CanJump(){
		return !isJumping && !InWeb ();
	}

	bool CanMove(){
		return !isJumping && !InWeb ();
	}
	
	void BurstLight(){
		playerLight.range = maxSpotAngle * jumpLightMultiplier;
	}
	
	void StayOnTheFloor(){
		if(isJumping && transform.position.y <= startPosition.y){
			isJumping = false;
			rbody.useGravity = false;
			Physics.gravity = Vector3.down * -9.81f;
		} else {
			transform.position = new Vector3(transform.position.x, startPosition.y, transform.position.z);
		}
	}

	public void Hit(float damage){
		hurtSound.Play();
		energy -= damage;
	}

	public void WebSnare(GameObject web){
		webSnareSound.Play();
		webSnare = web;
	}

	public bool InWeb(){
		return webSnare != null;
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
