using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TopDownSpriteZOrder : MonoBehaviour {

	private SpriteRenderer spriteRenderer;

	void Awake () {
		spriteRenderer = GetComponent<SpriteRenderer> ();
	}

	// Update is called once per frame
	void LateUpdate () {
		if (spriteRenderer != null)
			spriteRenderer.sortingOrder = Mathf.RoundToInt(transform.position.y) * -1;
	}
}
