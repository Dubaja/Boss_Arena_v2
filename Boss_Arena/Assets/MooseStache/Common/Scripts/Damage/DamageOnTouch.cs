using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageOnTouch : MonoBehaviour {

	[Header("Targets")]
	public string TargetTag = "Entity";

	[Header ("Damage")]
	public int DamageToCause = 1;

	[Header ("Owner")]
	public GameObject Owner;

	protected virtual void Awake () {
		if (Owner == null) {
			Owner = gameObject;
		}
	}

	public virtual void OnTriggerEnter2D(Collider2D collider)
	{			
		Colliding (collider);
	}

	public virtual void OnTriggerStay2D(Collider2D collider)
	{			
		Colliding (collider);
	}

	protected virtual void Colliding(Collider2D collider)
	{
		if (!isActiveAndEnabled) {
			return;
		}

		// if what we're colliding with isn't the target tag, we do nothing and exit
		if (!collider.gameObject.CompareTag(TargetTag)) {
			return;
		}

		var health = collider.gameObject.GetComponent<Health>();

		// If what we're colliding with is damageable / Has  health component
		if (health != null)
		{
			if(health.health > 0 && !health.invincible)
			{
				// Apply the Damage
				health.TakeDamage(DamageToCause);
			}
		} 
	}
}
