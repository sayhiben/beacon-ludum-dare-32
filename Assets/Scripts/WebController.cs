using UnityEngine;
using System.Collections;

public class WebController : MonoBehaviour {

	public int maxHealth = 3;

	private GameObject player;
	private int health;

	void Start(){
		player = GameObject.Find("Player");
		health = maxHealth;
	}

	void OnTriggerEnter(Collider other){
		if(other.gameObject.tag == "Player Shot"){
			HandleHits(other);
		} else if(other.gameObject.tag == "Player"){
			HandlePlayer(other);
		}
	}

	void HandleHits(Collider other){
		health--;
		if(health <= 0){
			Destroy (gameObject);
		}
	}

	void HandlePlayer(Collider other) {
		other.gameObject.GetComponent<PlayerController>().WebSnare(gameObject);
	}
}
