using UnityEngine;
using System.Collections;

public class TextureTiler : MonoBehaviour {
	// Update is called once per frame
	void Update () {
		GetComponent<Material>().mainTextureScale = new Vector2(transform.localScale.x, transform.localScale.y);
	}
}
