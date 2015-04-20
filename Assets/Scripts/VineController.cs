using UnityEngine;
using System.Collections;

public class VineController : MonoBehaviour {

	public AudioClip destroySound;

	void OnTriggerEnter(Collider other){
		if(other.gameObject.tag == "Player Shot"){
			if(destroySound){
				AudioSource.PlayClipAtPoint(destroySound, transform.position, 0.7f);
			}
			Destroy (gameObject);
		}
	}

}
