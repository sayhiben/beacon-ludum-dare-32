using UnityEngine;
using System.Collections;

public class FrogController : MonoBehaviour {

	public float attackFrequency = 1.5f;
	public float hitDamage = 20.0f;
	public float tongueOutSpeed = 1.5f;
	public float tongueInSpeed = 1.0f;
	public float maxTongueLength = 10.0f;

	public GameObject tongue;

	private bool isAttacking = false;
	private float nextAttack;
	private float originalTongueLength;
	private bool isExpanding = false;

	void Start(){
		originalTongueLength = tongue.transform.localScale.z;
	}

	void FixedUpdate () {
		if(!isAttacking && Time.time >= nextAttack){
			isAttacking = true;
			isExpanding = true;
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
			tongue.transform.localScale = new Vector3(
				tongue.transform.localScale.x, 
				tongue.transform.localScale.y, 
				tongue.transform.localScale.z + expandSpeed * Time.deltaTime
			);

			// adjust position based on scale
			tongue.transform.position = new Vector3(
				tongue.transform.position.x,
				tongue.transform.position.y,
				tongue.transform.position.z - expandSpeed * Time.deltaTime * 5.0f
				);

			// check for contracted all the way and stop attacking
			if(!isExpanding && tongue.transform.localScale.z <= originalTongueLength){
				tongue.transform.localScale = new Vector3(
					tongue.transform.localScale.x, 
					tongue.transform.localScale.y, 
					originalTongueLength
				);
				isAttacking = false;
				nextAttack = Time.time + attackFrequency;
			}
		}
	}
}
