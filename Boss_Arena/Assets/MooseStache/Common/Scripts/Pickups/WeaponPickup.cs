using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponPickup : Interactable {

	public Weapon wep;
	public Animator scrollAnimator;

	new void Update () {
		if (!wasInside && isInside) {
			if (!scrollAnimator.GetCurrentAnimatorStateInfo (0).IsName ("ScrollAppear")) {
				scrollAnimator.Play ("ScrollAppear");
			}
		} else if (wasInside && !isInside) {
			if (!scrollAnimator.GetCurrentAnimatorStateInfo (0).IsName ("ScrollDisappear")) {
				scrollAnimator.Play ("ScrollDisappear");
			}
		}

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
		player.EquipWeapon (wep);
		Destroy (gameObject);
	}
}
