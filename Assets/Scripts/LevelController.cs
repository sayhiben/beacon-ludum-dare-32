﻿using UnityEngine;
using System.Collections;

public class LevelController : MonoBehaviour {

	public string nextSceneName;
	public Texture fadeOutTexture;
	public Texture restartTexture;
	public float fadeSpeed = 0.3f;
	public bool fadeInOnStart = true;

	private float alpha = 1.0f;
	private int fadeDir = -1;
	private int drawDepth = -1000;
	private bool isTransitioning = false;

	public bool IsTransitioning(){
		return isTransitioning;
	}

	void FadeIn(){
		fadeDir = -1;
	}

	void FadeOut(){
		fadeDir = 1;
	}

	void Start(){
		if(fadeInOnStart){
			alpha = 1;
			FadeIn();
			isTransitioning = true;
		} else {
			alpha = 0.01f;
		}
	}

	public void RestartLevel(){
		if(!isTransitioning){
			fadeOutTexture = restartTexture;
			nextSceneName = Application.loadedLevelName;
			FadeOut ();
			isTransitioning = true;
		}
	}

	public void NextLevel(){
		if(!isTransitioning){
			FadeOut ();
			isTransitioning = true;
		}
	}

	void Update(){
		if(!isTransitioning && Input.GetKeyDown(KeyCode.Escape) && Application.loadedLevelName != "start"){
			nextSceneName = "start";
			NextLevel();
		}
	}

	void OnLevelWasLoaded(int levelIndex){
		alpha = 1;
		FadeIn();
		isTransitioning = true;
	}

	void OnGUI(){
		if(isTransitioning){
			alpha += fadeDir * fadeSpeed * Time.deltaTime;  
			alpha = Mathf.Clamp01(alpha);   

			Color color = GUI.color;
			color.a = alpha;
			GUI.color = color;
			
			GUI.depth = drawDepth;
			
			GUI.DrawTexture(new Rect(0, 0, Screen.width, Screen.height), fadeOutTexture);
			if (fadeDir > 0 && alpha >= 0.99f) {
				Application.LoadLevel(nextSceneName);
			} else if (alpha < 0.01f){
				isTransitioning = false;
			}
		}
	}
}
