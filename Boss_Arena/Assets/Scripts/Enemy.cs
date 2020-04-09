using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
	public Rigidbody2D playerRb;
	public Rigidbody2D enemyRb;
	public float maxHealth = 100f;
	//private float currentHealth;
	public GameObject deathEffect;

	void FixedUpdate(){
    	//rb.MovePosition(rb.position + movement * moveSpeed * Time.fixedDeltaTime);

    	Vector2 lookDir = playerRb.position - enemyRb.position;
    	float angle = Mathf.Atan2(lookDir.y, lookDir.x) * Mathf.Rad2Deg - 90f;
    	enemyRb.rotation = angle;
    }

	public void TakeDamage(float damage){
		maxHealth -= damage;

		if (maxHealth <= 0){
			die();
		}
	}

	void die(){
		GameObject effect = Instantiate(deathEffect, transform.position, Quaternion.identity);
		Destroy(effect, .5f);
		Destroy(gameObject);
	}
}
