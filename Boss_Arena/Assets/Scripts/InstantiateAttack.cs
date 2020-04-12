using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InstantiateAttack : MonoBehaviour
{
    public GameObject attack;
    public GameObject attackEff;

    public void Attack(){
        GameObject ob = Instantiate(attack , GetComponent<Transform>().position, Quaternion.identity);
        GameObject effect = Instantiate(attackEff, GetComponent<Transform>().position, Quaternion.identity);
		Destroy(effect, .5f);
        Destroy(ob, .1f);
    }
}
