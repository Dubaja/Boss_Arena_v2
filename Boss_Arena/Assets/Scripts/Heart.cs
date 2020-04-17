using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Heart : MonoBehaviour
{
    void OnTriggerEnter2D(Collider2D col){
        if(col.tag == "Player"){
            FindObjectOfType<Player1>().HealPlayer(20f);
            Destroy(gameObject);
        }
    }
}
