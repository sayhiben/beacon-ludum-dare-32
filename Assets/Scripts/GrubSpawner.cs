using UnityEngine;
using System.Collections;

public class GrubSpawner : MonoBehaviour {

	public float healthToDrop = 0.0f;

	private bool isQuitting = false;

	void OnApplicationQuit(){
		isQuitting = true;
	}
	
	void OnDestroy(){
		if(healthToDrop > 0){
			GameObject healthGrub = (GameObject)Instantiate(Resources.Load("Health Grub"));
			healthGrub.GetComponent<HealthGrubController>().healAmount = healthToDrop;
			healthGrub.transform.position = transform.position;
		}
	}
}
