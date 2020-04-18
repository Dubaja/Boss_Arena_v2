using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUp : MonoBehaviour
{
    public GameObject anim;

    void OnTriggerEnter2D(Collider2D collider){
        if(collider.tag == "Player"){
            GameObject eff = Instantiate(anim, transform.position, Quaternion.identity);
            Destroy(eff, 0.5f);
            FindObjectOfType<Buff>().powerUpFlag = true;
            Destroy(gameObject);
        }
    }

}
