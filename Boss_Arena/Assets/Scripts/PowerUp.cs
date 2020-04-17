using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUp : MonoBehaviour
{
    void OnTriggerEnter2D(Collider2D collider){
        if(collider.tag == "Player"){
            //instantiate animation
            FindObjectOfType<Buff>().powerUpFlag = true;
            Destroy(gameObject);
        }
    }

}
