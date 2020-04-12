using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHp : MonoBehaviour
{
    public float maxHealth = 100f;
    public GameObject deathEffect;

    public void TakeDamage(float damage){
		maxHealth -= damage;

		if (maxHealth <= 0){
			Die();
		}
	}

	void Die(){
		GameObject effect = Instantiate(deathEffect, transform.position, Quaternion.identity);
		Destroy(effect, .5f);
		Destroy(gameObject);
	}
}
