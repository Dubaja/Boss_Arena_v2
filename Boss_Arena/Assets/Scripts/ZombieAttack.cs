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

    void Start(){
        timeBtwAttack = startTimeBtwAttack;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (timeBtwAttack <= 0){
            if (GetComponent<ZombieMovement>().GetLookDirMagnitude() < 2f){
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
