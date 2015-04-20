using UnityEngine;
using System.Collections;

public class HornetController : EnemyController {
		
	public GameObject bodyPlane;
	public Texture[] bodyTextures;
	public float animationSpeed = 0.15f;
	public float speed;
	public float health = 2;
	public float healthToDrop = 10.0f;
	
	public AudioClip deathSound;
	
	private int bodyTexture = 0;
	private float nextFrame;
	private bool isQuitting = false;
	private Rigidbody rbody;
	
	void AnimateFlying(){
		if(Time.time >= nextFrame){
			MeshRenderer renderer = bodyPlane.GetComponent<MeshRenderer>();
			renderer.material.mainTexture = bodyTextures[bodyTexture];
			bodyTexture++;
			if(bodyTexture >= bodyTextures.Length){
				bodyTexture = 0;
			}
			nextFrame = Time.time + animationSpeed;
		}
	}

	public override void Hit(){
		health--;
		if(health <= 0){
			Destroy(gameObject);
		}
	}

	void OnApplicationQuit(){
		isQuitting = true;
	}
	
	// Use this for initialization
	void Start () {
		rbody = GetComponent<Rigidbody>();
		rbody.AddForce(transform.forward * speed);
	}
	
	// Update is called once per frame
	void Update () {
		AnimateFlying();
	}

	void OnDestroy(){
		if(!isQuitting){
			AudioSource.PlayClipAtPoint(deathSound, transform.position);
			if(healthToDrop > 0){
				GameObject healthGrub = (GameObject)Instantiate(Resources.Load("Health Grub"));
				healthGrub.GetComponent<HealthGrubController>().healAmount = healthToDrop;
				healthGrub.transform.position = transform.position;
			}
		}
	}
}
