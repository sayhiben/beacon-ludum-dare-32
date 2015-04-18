using UnityEngine;
using System.Collections;

public class VineController : MonoBehaviour {

	void OnTriggerEnter(Collider other){
		if(other.gameObject.tag == "Player Shot"){
			Destroy (gameObject);
		}
	}

}
