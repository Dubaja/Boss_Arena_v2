using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{

	public GameObject hitEffect;
	private float zemicke;
	// private float baseDamage = 20f;
	// public float damage;

	void Start(){
		zemicke = FindObjectOfType<Shooting>().damage;
	}

	//this function deals damage on collision with enemy
	void OnTriggerEnter2D(Collider2D hitInfo){
		GameObject enemy = hitInfo.gameObject;

		if(enemy != null && enemy.CompareTag("Enemy")){
			enemy.GetComponent<EnemyHp>().TakeDamage(zemicke);
		}

		if(!enemy.CompareTag("Drop")){
			GameObject effect = Instantiate(hitEffect, transform.position, Quaternion.identity);
			Destroy(effect, .4f);
			Destroy(gameObject);
		}
	}

	// public void SetDamage(float factor){
	// 	damage *= factor;
	// }

	// public void SetToBaseDamage(){
	// 	damage = baseDamage;
	// }
}
