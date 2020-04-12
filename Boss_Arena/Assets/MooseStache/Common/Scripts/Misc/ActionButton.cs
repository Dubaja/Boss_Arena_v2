using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActionButton : MonoBehaviour {

	public bool fading = false;
	public Animator animator;

	public void FadeMe () {
		fading = true;

		if (!animator.GetCurrentAnimatorStateInfo (0).IsName ("FadeOut")) {
			animator.Play ("FadeOut");
		}
	}

	public void DestroySelf () {
		if (!fading)
			Destroy (gameObject);
	}
}
