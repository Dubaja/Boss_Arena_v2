using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZombieAttack : MonoBehaviour
{
    private float timeBtwAttack;
    // public GameObject attackEffect;
    public Transform attackPos;
    public LayerMask whatIsPlayer;
    public float startTimeBtwAttack = 0.5f;
    public float attackRange;
    public float damage;
    Vector2 lookDir;
    GameObject player;

    void Start(){
        timeBtwAttack = startTimeBtwAttack;
        player = GameObject.Find("Player");
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if(player != null){
            lookDir = new Vector2(player.transform.position.x - gameObject.transform.position.x, player.transform.position.y - gameObject.transform.position.y);
        }
        if (timeBtwAttack <= 0){
            if (lookDir.magnitude < 2f){
                Debug.Log("Napadam :D");
                Collider2D playersToDamage = Physics2D.OverlapCircle(attackPos.position, attackRange, whatIsPlayer);
                if(playersToDamage != null){
                    playersToDamage.GetComponent<Player1>().playerTakeDamage(damage);
                    // GameObject effect = Instantiate(attackEffect, attackPos.position, Quaternion.identity);
		            // Destroy(effect, .5f);
                }
                timeBtwAttack = startTimeBtwAttack;
            }
        }else{
            timeBtwAttack -= Time.deltaTime;
        }
    }

    void OnDrawGizmosSelected(){
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(attackPos.position, attackRange);
    }
}
