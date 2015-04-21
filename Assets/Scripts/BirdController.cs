using UnityEngine;
using System.Collections;

public class BirdController : MonoBehaviour {

	public GameObject wingsPlane;
	public Texture[] wingsTextures;
	public float flySpeed = 12.0f;
	public float animationSpeed = 0.15f;
	public int health = 2;
	public float damage = 20.0f;
	public float disappearDistance = 20.0f;
	public float aggroDistance = 3.0f;
	public float attackDistance = 2.0f;
	public float attackDelay = 1.0f;
	public float attackInSpeed = 1.0f;
	public float attackOutSpeed = 1.0f;
	public float snapDistance = 0.5f;
	public float healthToDrop = 0.0f;
	public GameObject lootAnchor;
	public AudioSource attackSound;
	public AudioSource flyAwaySound;
	
	private GameObject player;
	private int wingsTexture = 0;
	private bool isFlying = false;
	private bool isAttacking = false;
	private float nextFlyFrame;
	private Vector3 originalPosition;
	private float nextAttackTime;
	private bool isReaching = false;
	private bool isQuitting;

	// Use this for initialization
	void Start () {
		originalPosition = transform.position;
		player = GameObject.Find ("Player");
	}
	
	// Update is called once per frame
	void FixedUpdate () {
		if(isFlying){
			Fly();
		} else if(InAttackRange () && !isAttacking && Time.time >= nextAttackTime){
			isAttacking = true;
			isReaching = true;
			attackSound.Play ();
		}
		Animate();
	}

	void Animate(){
		if(isFlying){
			AnimateFlying();
		} else if(isAttacking){
			AnimateAttacking();
		}
	}

	void AnimateFlying(){
		if(Time.time >= nextFlyFrame){
			MeshRenderer renderer = wingsPlane.GetComponent<MeshRenderer>();
			renderer.material.mainTexture = wingsTextures[wingsTexture];
			wingsTexture++;
			if(wingsTexture >= wingsTextures.Length){
				wingsTexture = 0;
			}
			nextFlyFrame = Time.time + animationSpeed;
		}
	}

	void AnimateAttacking(){
		// determine direction
		float expandSpeed = attackOutSpeed;
		if(!isReaching){
			expandSpeed = -attackInSpeed;
		} else if(ReachDistance () >= attackDistance) {
			isReaching = false;
		}

		// adjust position based on scale
		transform.position += transform.forward * expandSpeed;
		
		// check for contracted all the way and stop attacking
		if(!isReaching && ReachDistance() <= snapDistance){
			transform.position = originalPosition;
			isAttacking = false;
			nextAttackTime = Time.time + attackDelay;
		}
	}

	void Fly(){
		Rigidbody rigidbody = GetComponent<Rigidbody>();
		rigidbody.velocity = transform.forward * flySpeed;

		if(Vector3.Distance(originalPosition, transform.position) >= disappearDistance){
			Destroy (gameObject);
		}
	}

	float ReachDistance(){
		return Vector3.Distance(transform.position, originalPosition);
	}

	bool InAttackRange(){
		return Vector3.Distance(player.transform.position, transform.position) <= aggroDistance;
	}
	
	void OnApplicationQuit(){
		isQuitting = true;
	}

	void OnTriggerEnter(Collider other){
		if(other.tag == "Player"){
			other.GetComponent<PlayerController>().Hit(damage);
		} else if(other.tag == "Player Shot"){
			Destroy (other.gameObject);
			health--;
			if(health <= 0){
				if(!isQuitting && !Application.isLoadingLevel && healthToDrop > 0){
					GameObject healthGrub = (GameObject)Instantiate(Resources.Load("Health Grub"));
					healthGrub.GetComponent<HealthGrubController>().healAmount = healthToDrop;
					healthGrub.transform.position = lootAnchor.transform.position;
				}
				flyAwaySound.Play();
				isFlying = true;
			}
		}
	}
}
