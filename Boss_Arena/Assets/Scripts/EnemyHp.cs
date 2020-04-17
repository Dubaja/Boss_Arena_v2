using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHp : MonoBehaviour
{
    public float maxHealth = 100f;
    public GameObject deathEffect;
	public GameObject drop;
	public GameObject heart;

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
		int num = Random.Range(1, 100+1);
		if(num < 11){
			Instantiate(drop, transform.position, Quaternion.identity);
		}
		if(num >= 11 && num < 21){
			Instantiate(heart, transform.position, Quaternion.identity);
		}
	}
}
