using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHp : MonoBehaviour
{
    public float maxHealth = 100f;
    public GameObject deathEffect;
	public GameObject drop;

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
		DropItem();
	}

	void DropItem(){
		if(Random.Range(1,10) == 1){
			Instantiate(drop, transform.position, Quaternion.identity);
		}
	}
}
