using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
	GameObject player;
	Rigidbody2D enemyRb;
	public float maxHealth = 100f;
	public float speed = 2f;
	
	public float enemyDmg = 25f;

	public GameObject deathEffect;
	
	public float hitDelta = 0.5f;
	private float myTime = 0.0f;
	private float nextHit = 0.5f;

	void Start(){
		player = GameObject.Find("Player");
		enemyRb = GetComponent<Rigidbody2D>();
	}

	void FixedUpdate(){
		MoveMonster(); 	
    }

	void MoveMonster(){
		if(player != null){
			Vector2 lookDir = new Vector2(player.transform.position.x - enemyRb.position.x, player.transform.position.y - enemyRb.position.y);
			float angle = Mathf.Atan2(lookDir.y, lookDir.x) * Mathf.Rad2Deg - 90f;
			
			Vector2 target = lookDir.normalized;
			//target.Normalize();
			enemyRb.rotation = angle;
			if (lookDir.magnitude > 1.0f){				
				enemyRb.MovePosition(enemyRb.position + target * speed * Time.fixedDeltaTime);
			}

			// if (lookDir.magnitude <= 2f){
			// 	myTime = myTime + Time.deltaTime;

			// 	if (myTime > nextHit)
			// 	{
			// 		Debug.Log("napao sam :)");
			// 		FindObjectOfType<InstantiateAttack>().Attack();
			// 		nextHit = myTime + hitDelta;
			// 		nextHit = nextHit - myTime;
			// 		myTime = 0.0f;
			// 	}
			// }
		}
	}

	void OnTrigger2D(Collider2D col){
        Player1 pl = col.GetComponent<Player1>();
		myTime = myTime + Time.deltaTime;
		if(pl != null && myTime > nextHit){
			pl.playerTakeDamage(enemyDmg);
			Debug.Log("napao sam :)");
			//FindObjectOfType<InstantiateAttack>().Attack();
			nextHit = myTime + hitDelta;
			nextHit = nextHit - myTime;
			myTime = 0.0f;
		}
    }

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
