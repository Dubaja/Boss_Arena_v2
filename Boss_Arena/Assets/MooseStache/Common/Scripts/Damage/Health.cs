using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Health : MonoBehaviour {

	public UnityEvent OnTakeDamageEvent;
	public UnityEvent OnTakeHealEvent;
	public UnityEvent OnDeathEvent;

	[Header ("Max/Starting Health")]
	public int maxHealth;
	[Header ("Current Health")]
	public int health;

	[Header ("IsDeathOrNot")]
	public bool dead = false;

	[Header ("Invincible")]
	public bool invincible = false;
	public bool becomeInvincibleOnHit = false;
	public float invincibleTimeOnHit = .5f;
	private float invincibleTimer = 0f;

	[Header ("Perform Dead Events after x time")]
	public float DieEventsAfterTime = 1f;

	void Start () {
		health = maxHealth;
	}

	void Update () {
		if (invincibleTimer > 0f) {
			invincibleTimer -= Time.deltaTime;

			if (invincibleTimer <= 0f) {
				if (invincible)
					invincible = false;
			}
		}
	}

	public bool TakeDamage (int amount) {
		if (dead || invincible)
			return false;

		health = Mathf.Max (0, health - amount);

		if (OnTakeDamageEvent != null)
			OnTakeDamageEvent.Invoke();

		if (health <= 0) {
			Die ();
		} else {
			if (becomeInvincibleOnHit) {
				invincible = true;
				invincibleTimer = invincibleTimeOnHit;
			}	 
		}

		return true;
	}

	public bool TakeHeal (int amount) {
		if (dead || health == maxHealth)
			return false;

		health = Mathf.Min (maxHealth, health + amount);

		if (OnTakeHealEvent != null)
			OnTakeHealEvent.Invoke();


		return true;
	}

	public void Die () {
		dead = true;

		if (CameraShaker.instance != null) {
			CameraShaker.instance.InitShake(0.2f, 1f);
		}

		StartCoroutine (DeathEventsRoutine (DieEventsAfterTime));
	}

	IEnumerator DeathEventsRoutine (float time) {
		yield return new WaitForSeconds (time);
		if (OnDeathEvent != null)
			OnDeathEvent.Invoke();
	}

	public void SetUIHealthBar () {
		if (UIHeartsHealthBar.instance != null) {
			UIHeartsHealthBar.instance.SetHearts (health);
		}
	}
}
