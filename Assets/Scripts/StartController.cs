using UnityEngine;
using System.Collections;

public class StartController : MonoBehaviour {
	// Update is called once per frame
	void Update () {
		if(Input.GetKeyDown(KeyCode.Return)){
			GameObject.Find("Level Manager").GetComponent<LevelController>().NextLevel();
		} else if (Input.GetKeyDown(KeyCode.Escape)){
			Application.Quit();
		}
	}
}
