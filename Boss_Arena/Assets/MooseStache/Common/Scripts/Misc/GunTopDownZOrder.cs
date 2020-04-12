using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunTopDownZOrder : MonoBehaviour {

	public SpriteRenderer targetSpriteRenderer;
	public int offSet = 1;

	private SpriteRenderer spriteRenderer;

	void Awake () {
		if (targetSpriteRenderer == null) {
			Debug.Log ("This Gun Z Order script has no target sprite renderer");
		}

		spriteRenderer = GetComponent<SpriteRenderer> ();
	}

	// Update is called once per frame
	void LateUpdate () {
		if (targetSpriteRenderer != null) {
			if (spriteRenderer != null)
				spriteRenderer.sortingOrder = targetSpriteRenderer.sortingOrder + offSet;
		}
	}
}
