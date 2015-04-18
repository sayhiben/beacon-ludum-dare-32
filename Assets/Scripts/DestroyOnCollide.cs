using UnityEngine;
using System.Collections;

public class DestroyOnCollide : MonoBehaviour {

	void OnTriggerEnter(Collider other){
		if(other.gameObject.tag == "Wall") {
			Destroy(gameObject);
		}
	}
}
