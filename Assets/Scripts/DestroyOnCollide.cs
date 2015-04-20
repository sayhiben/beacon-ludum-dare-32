using UnityEngine;
using System.Collections;

public class DestroyOnCollide : MonoBehaviour {

	public AudioClip hitSound;

	void OnTriggerEnter(Collider other){
		if(other.gameObject.tag == "Wall" || other.gameObject.tag == "Enemy") {
			Destroy(gameObject);
		}

		if(other.gameObject.tag == "Enemy") {
			if(hitSound){
				AudioSource.PlayClipAtPoint(hitSound, other.transform.position, 0.5f);
			}
			other.GetComponent<EnemyController>().Hit();
		}
	}
}
