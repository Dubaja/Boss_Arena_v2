using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player1 : MonoBehaviour
{
    public GameObject hitEff;
    
    public float maxHealth = 100f;
    public float health;

    // Start is called before the first frame update
    void Start()
    {
        health = maxHealth;
        FindObjectOfType<HealthBar>().SetMaxHealth(health);
    }

    public void playerTakeDamage(float damage){
        Debug.Log("Udario me :*(");
        GameObject effect = Instantiate(hitEff, transform.position, Quaternion.identity);
		Destroy(effect, .3f);
        health -= damage;
        FindObjectOfType<HealthBar>().SetHealth(health);
        if(health <= 0){
            PlayerDie();
        }
    }

    public void HealPlayer(float hp){
        Debug.Log("Heal!");
        if((health + hp) > maxHealth){
            health = maxHealth;
        }else{
            health += hp;
        }
        FindObjectOfType<HealthBar>().SetHealth(health);
    }

    void PlayerDie(){
        Destroy(gameObject);
    }
}
