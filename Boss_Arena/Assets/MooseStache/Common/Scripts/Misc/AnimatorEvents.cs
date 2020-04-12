using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimatorEvents : MonoBehaviour {

	public void DestroyMe () {
		Destroy (gameObject);
	}

	public void DestroyParent () {
		Destroy (transform.parent.gameObject);
	}

	public void TriggerParentInteractableAction () {
		var comp = GetComponentInParent<Interactable> ();

		if (comp != null) {
			comp.TriggerAction ();
		}
	}
}
