using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Crate : MonoBehaviour {

	public int hitsTaken = 0;
	public int maxHitsNumber = 3;

	public Sprite[] sprites;

	public SpriteRenderer spriteRenderer;

	public BoxCollider2D myCollider;

	public GameObject DestroyFX;

	public void TakeHit () {
		hitsTaken = Mathf.Min (hitsTaken + 1, maxHitsNumber);

		// Camera Shake
		if (CameraShaker.instance != null) {
			CameraShaker.instance.InitShake(0.075f, 1f);
		}

		// Set the right sprite
		if (spriteRenderer != null) {
			spriteRenderer.sprite = sprites [hitsTaken];
		}
	}

	public void Die () {
		// Disable the collider
		if (myCollider != null) {
			myCollider.enabled = false;
		}

		// Instantiate the destroy FX
		if (DestroyFX != null) {
			Instantiate (DestroyFX, transform.position, Quaternion.identity);
		}

		// Camera Shake
		if (CameraShaker.instance != null) {
			CameraShaker.instance.InitShake(0.12f, 1f);
		}

		// Destroy the gameobject
		Destroy (gameObject);
	}

}
