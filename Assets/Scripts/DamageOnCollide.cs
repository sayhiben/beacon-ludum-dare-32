using UnityEngine;
using System.Collections;

public class DamageOnCollide : MonoBehaviour {

	public float damage = 10.0f;
	public bool enablePushback = true;
	public float pushbackPower = 5.0f;

	void OnTriggerEnter(Collider other){
		if(other.tag == "Player"){
			if(enablePushback){
				Vector3 pushbackDirection = (other.transform.position - transform.position).normalized;
				pushbackDirection.y = 0;
				other.GetComponent<Rigidbody>().AddForce(pushbackDirection * pushbackPower);
			}
			other.GetComponent<PlayerController>().Hit(damage);
		}
	}
}
