using UnityEngine;
using System.Collections;

public class HealthGrubController : MonoBehaviour {
	public float healAmount = 15.0f;
	public Light glowLight;
	public float minGlowIntensity = 2.0f;
	public float maxGlowIntensity = 4.0f;
	public float glowSpeed = 0.15f;

	private bool increasingGlow = true;

	void Update(){
		float glowChange = glowSpeed * Time.deltaTime;
		if(!increasingGlow){
			glowChange *= -1;
		}
		glowLight.intensity += glowChange;

		// change directions
		if(glowLight.intensity >= maxGlowIntensity){
			increasingGlow = false;
		} else if (glowLight.intensity <= minGlowIntensity){
			increasingGlow = true;
		}
	}

	void OnTriggerEnter(Collider other){
		if(other.tag == "Player"){
			other.GetComponent<PlayerController>().Heal(healAmount);
			Destroy (gameObject);
		}
	}
}
