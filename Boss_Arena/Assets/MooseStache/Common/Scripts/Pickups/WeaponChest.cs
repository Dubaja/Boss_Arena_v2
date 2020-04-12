using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponChest : Interactable {

	public Animator animator;

	public WeaponPickup[] weaponPickups;

	new void Update () {
		base.Update ();
	}

	void OnTriggerEnter2D(Collider2D col) {
		OnTriggerEnter2DFunc (col);
	}

	void OnTriggerExit2D(Collider2D col) {
		OnTriggerExit2DFunc (col);
	}

	/*
	void OnTriggerStay2D (Collider2D col) {
		OnTriggerStay2DFunc (col);
	}
	*/
	protected override void OnPlayerTrigger (Player player)
	{
		base.OnPlayerTrigger (player);
		if (!animator.GetCurrentAnimatorStateInfo (0).IsName ("Open")) {
			animator.Play ("Open");
		}
	}

	public override void TriggerAction () {
		if (weaponPickups.Length > 0) {
			Instantiate (weaponPickups [Random.Range (0, weaponPickups.Length)], transform.position + new Vector3(0f, -1, 0f), Quaternion.identity);
		} else {
			Debug.Log ("This weapon chest has no pickable weapons");
		}
	}
}
