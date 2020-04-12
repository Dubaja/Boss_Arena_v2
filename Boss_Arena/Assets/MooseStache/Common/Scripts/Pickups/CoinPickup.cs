using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinPickup : Interactable {

	public GameObject PickFxPrefab;

	// Update is called once per frame
	new void Update () {
		base.Update ();
	}

	void OnTriggerEnter2D(Collider2D col) {
		OnTriggerEnter2DFunc (col);
	}

	void OnTriggerStay2D(Collider2D col) {
		OnTriggerStay2DFunc (col);
	}

	void OnTriggerExit2D(Collider2D col) {
		OnTriggerExit2DFunc (col);
	}

	protected override void OnPlayerTrigger (Player player)
	{
		base.OnPlayerTrigger (player);

		// Add the actual score/currency
		if (ScoreManager.Instance != null) {
			ScoreManager.Instance.AddScore (1);
		}

		// Instantiate the pickup fx
		if (PickFxPrefab != null) {
			Instantiate (PickFxPrefab, transform.position, Quaternion.identity);
		}

		// Camera Shake
		if (CameraShaker.instance != null) {
			CameraShaker.instance.InitShake(0.05f, 1f);
		}

		// Destroy the gameobject
		Destroy (gameObject);
	}
}
