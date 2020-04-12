using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIWeaponIcon : MonoBehaviour {

	public Image imageRenderer;

	public static UIWeaponIcon instance = null;

	void Awake () {
		if (instance == null) {
			instance = this;
		} else if (instance != this) {
			Destroy (gameObject);
		}

		if (imageRenderer == null) {
			imageRenderer = GetComponent<Image> ();
		}
	}

	public void SetWeaponIcon (Sprite sprite) {
		imageRenderer.sprite = sprite;
		imageRenderer.SetNativeSize ();
	}
}
