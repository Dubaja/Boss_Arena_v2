using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpriteFlasher : MonoBehaviour {

	public float flashTime = 0.1f;

	public float flashamount = 1f;

	public SpriteRenderer spriteRenderer;

	IEnumerator flashRoutine;

	public void FlashMe () {
		if (flashRoutine != null) {
			StopCoroutine (flashRoutine);
		}

		flashRoutine = FlashRoutine ();
		StartCoroutine (flashRoutine);
	}

	IEnumerator FlashRoutine () {
		spriteRenderer.material.SetFloat ("_FlashAmount", flashamount);

		yield return new WaitForSeconds (flashTime);

		spriteRenderer.material.SetFloat ("_FlashAmount", 0f);
	}
}
