using UnityEngine;
using System.Collections;

public class DamageOnCollide : MonoBehaviour {

	public float damage = 10.0f;
	public bool enablePushback = true;
	public float pushbackPower = 5.0f;
	public AudioSource hitSound;

	void OnTriggerEnter(Collider other){
		if(other.tag == "Player"){
			if(enablePushback){
				Vector3 pushbackDirection = (other.transform.position - transform.position).normalized;
				pushbackDirection.y = 0;
				other.GetComponent<Rigidbody>().AddForce(pushbackDirection * pushbackPower);
			}
			if(hitSound != null){
				hitSound.Play ();
			}
			other.GetComponent<PlayerController>().Hit(damage);
		}
	}
}
