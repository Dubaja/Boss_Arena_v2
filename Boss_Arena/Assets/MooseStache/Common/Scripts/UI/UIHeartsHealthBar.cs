using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIHeartsHealthBar : MonoBehaviour {

	public Image[] sprites;

	public static UIHeartsHealthBar instance = null;

	void Awake () {
		if (instance == null) {
			instance = this;
		} else if (instance != this) {
			Destroy (gameObject);
		}
	}

	public void SetHearts(int amount) {
		for (int i = 0; i < sprites.Length; i++) {
			if (i < amount) {
				if (!sprites [i].enabled)
					sprites [i].enabled = true;
			} else {
				if (sprites [i].enabled)
					sprites [i].enabled = false;
			}
		}
	}
}
