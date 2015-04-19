using UnityEngine;
using System.Collections;

public class SpiderShotController : MonoBehaviour {

	public float distanceToLive = 50.0f;

	public GameObject webTrap;

	private Vector3 startPosition;

	void Awake(){
		startPosition = transform.position;
	}

	void Update(){
		if(Vector3.Distance(startPosition, transform.position) >= distanceToLive){
			Destroy (gameObject);
		}
	}

	void OnTriggerEnter(Collider other){
		if(other.tag == "Player"){
			Instantiate(webTrap, other.transform.position, Quaternion.identity);
			Destroy (gameObject);
		}
		if(other.tag == "Wall") {
			Destroy (gameObject);
		}
	}
	
}
