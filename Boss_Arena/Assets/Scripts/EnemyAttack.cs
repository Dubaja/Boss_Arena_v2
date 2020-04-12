using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAttack : MonoBehaviour
{
    GameObject player;

    public float enemyDmg = 25f;

    void Start()
    {
        player = GameObject.Find("Player");
    }

    void OnTriggerEnter2D(Collider2D col){
        Player1 pl = col.GetComponent<Player1>();

		if(pl != null){
			pl.playerTakeDamage(enemyDmg);
		}
    }
}
