using UnityEngine;
using System.Collections;

public class GoalController : MonoBehaviour {

	private GameObject levelManager;

	void Start(){
		levelManager = GameObject.Find("Level Manager");
	}

	void OnTriggerEnter(Collider other){
		if(other.tag == "Player"){
			levelManager.GetComponent<LevelController>().NextLevel();
		}
	}
}
