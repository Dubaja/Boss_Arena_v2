using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{

	public GameObject hitEffect;
	public float damage = 20f;

	//this function deals damage on collision with enemy
	void OnTriggerEnter2D(Collider2D hitInfo){
		GameObject enemy = hitInfo.gameObject;

		if(enemy != null){
			enemy.GetComponent<EnemyHp>().TakeDamage(damage);
		}
		GameObject effect = Instantiate(hitEffect, transform.position, Quaternion.identity);
		Destroy(effect, .5f);
		Destroy(gameObject);
	}
}
