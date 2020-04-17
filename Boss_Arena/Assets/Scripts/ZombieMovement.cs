using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZombieMovement : MonoBehaviour
{
    GameObject player;
	Rigidbody2D enemyRb;
    Vector2 lookDir;
    
    public float speed = 2f;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.Find("Player");
		enemyRb = GetComponent<Rigidbody2D>();
    }

    void FixedUpdate(){
		MoveMonster(); 	
    }

    void MoveMonster(){
		if(player != null){
			lookDir = new Vector2(player.transform.position.x - enemyRb.position.x, player.transform.position.y - enemyRb.position.y);
			float angle = Mathf.Atan2(lookDir.y, lookDir.x) * Mathf.Rad2Deg - 90f;
			Vector2 target = lookDir.normalized;
		    enemyRb.rotation = angle;
			
			if (lookDir.magnitude > 1f){				
				enemyRb.MovePosition(enemyRb.position + target * speed * Time.fixedDeltaTime);
			}
		}
	}
    
    public float GetLookDirMagnitude(){
        return lookDir.magnitude;
    }
}
