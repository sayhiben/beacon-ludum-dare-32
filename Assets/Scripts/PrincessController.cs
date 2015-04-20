using UnityEngine;
using System.Collections;

public class PrincessController : MonoBehaviour {


	public GameObject bodyPlane;
	public Texture[] bodyTextures;
	public float animationSpeed = 0.15f;
	private int bodyTexture = 0;
	private float nextFrame;

	void AnimateFlying(){
		if(Time.time >= nextFrame){
			MeshRenderer renderer = bodyPlane.GetComponent<MeshRenderer>();
			renderer.material.mainTexture = bodyTextures[bodyTexture];
			bodyTexture++;
			if(bodyTexture >= bodyTextures.Length){
				bodyTexture = 0;
			}
			nextFrame = Time.time + animationSpeed;
		}
	}


	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		AnimateFlying();
	}
}
