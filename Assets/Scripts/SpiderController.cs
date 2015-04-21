using UnityEngine;
using System.Collections;

public class SpiderController : EnemyController {

	public float speed = 5;
	public float fireDelay = 1.0f;
	public float rotateSpeed = 50.0f;
	public float aggroRange = 30.0f;
	public float health = 3;
	public float healthToDrop = 0.0f;
	
	public Transform shotSpawn;
	public Camera mainCamera;
	public GameObject spiderShot;
	public AudioClip deathSound;
	public AudioSource shootSound;

	private float nextFire;
	private GameObject player;
	private bool isQuitting;

	void Start(){
		player = GameObject.Find ("Player");
	}

	void OnApplicationQuit(){
		isQuitting = true;
	}
	
	void OnDestroy(){
		if(!isQuitting && Application.isLoadingLevel){
			AudioSource.PlayClipAtPoint(deathSound, transform.position);
			if(healthToDrop > 0){
				GameObject healthGrub = (GameObject)Instantiate(Resources.Load("Health Grub"));
				healthGrub.GetComponent<HealthGrubController>().healAmount = healthToDrop;
				healthGrub.transform.position = transform.position;
			}
		}
	}

	void Update(){
		transform.LookAt(player.transform.position);

		if(health > 0 && InRange() && Time.time >= nextFire && !player.GetComponent<PlayerController>().InWeb()){
			shootSound.Play();
			nextFire = Time.time + fireDelay;
			GameObject shot = (GameObject)Instantiate (spiderShot, shotSpawn.position, Quaternion.identity);
			shot.transform.LookAt(player.transform);
		}
	}

	bool InRange() {
		return Vector3.Distance(player.transform.position, transform.position) <= aggroRange;
	}

	public override void Hit(){
		health--;
		if(health <= 0){
			Destroy(gameObject);
		}
	}
}
