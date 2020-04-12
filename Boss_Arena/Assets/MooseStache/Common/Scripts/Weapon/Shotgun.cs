using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shotgun : Weapon {

	// Update is called once per frame
	new void Update () {
		base.Update ();
	}

	protected override void InstantiateProjectile ()
	{
		if (CameraShaker.instance != null) {
			CameraShaker.instance.InitShake(0.1f, 2f);
		}

		for (int i = 0; i < 5; i++) {
			base.InstantiateProjectile ();
		}
	}
}
