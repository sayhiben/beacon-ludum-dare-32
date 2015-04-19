using UnityEngine;
using System.Collections;

public class DestroyOnCollide : MonoBehaviour {

	void OnTriggerEnter(Collider other){
		if(other.gameObject.tag == "Wall" || other.gameObject.tag == "Enemy") {
			Destroy(gameObject);
		}

		if(other.gameObject.tag == "Enemy") {
			other.GetComponent<EnemyController>().Hit();
		}
	}
}
