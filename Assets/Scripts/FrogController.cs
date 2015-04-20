using UnityEngine;
using System.Collections;

public class FrogController : MonoBehaviour {

	public float attackFrequency = 1.5f;
	public float hitDamage = 20.0f;
	public float tongueOutSpeed = 1.5f;
	public float tongueInSpeed = 1.0f;
	public float maxTongueLength = 3.0f;

	public GameObject tongue;
	public AudioSource tongueSound;

	private bool isAttacking = false;
	private float nextAttack;
	private Vector3 originalScale;
	private Vector3 originalPosition;
	private bool isExpanding = false;

	void Start(){
		originalScale = tongue.transform.localScale;
		originalPosition = tongue.transform.position;
	}

	void FixedUpdate () {
		if(!isAttacking && Time.time >= nextAttack){
			isAttacking = true;
			isExpanding = true;
			if(!tongueSound.isPlaying){
				tongueSound.Play ();
			}
		}

		if(isAttacking){
			// determine direction
			float expandSpeed = tongueOutSpeed;
			if(!isExpanding){
				expandSpeed = -tongueInSpeed;
			} else if(tongue.transform.localScale.z >= maxTongueLength) {
				isExpanding = false;
			}

			// expand/contract
			tongue.transform.localScale += new Vector3(
				0.0f, 
				0.0f, 
				tongue.transform.localScale.z * expandSpeed * Time.deltaTime
			);

			// adjust position based on scale
			tongue.transform.position -= tongue.transform.forward * expandSpeed * 8.0f * Time.deltaTime;

			// check for contracted all the way and stop attacking
			if(!isExpanding && tongue.transform.localScale.z <= originalScale.z){
				tongue.transform.localScale = originalScale;
				tongue.transform.position = originalPosition;
				isAttacking = false;
				nextAttack = Time.time + attackFrequency;
			}
		}
	}
}
