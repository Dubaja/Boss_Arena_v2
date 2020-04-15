using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUp : MonoBehaviour
{
    // private float powerUpDuration = 15f;
    //public bool powerUpFlag = false;
    // private bool attackSpeedFlag;
    // private bool moveSpeedFlag;
    // private bool damageFlag;
    // private float attSpdTimer;
    // private float moveSpdTimer;
    // private float damageTimer;

    // void Start(){
    //     powerUpFlag = false;
    //     attackSpeedFlag = false;
    //     moveSpeedFlag = false;
    //     damageFlag = false;
    // }

    // void Update(){
    //     if(powerUpFlag){
    //         AddRandomPowerUp();
    //         powerUpFlag = false;
    //     }

    //     if(attSpdTimer <= 0 && attackSpeedFlag){
    //         RemovePowerUp("AttackSpeed");
    //     }else if(attSpdTimer > 0){
    //         attSpdTimer -= Time.deltaTime;
    //     }

    //     if(moveSpdTimer <= 0 && moveSpeedFlag){
    //         RemovePowerUp("MovementSpeed");
    //     }else if(moveSpdTimer > 0){
    //         moveSpdTimer -= Time.deltaTime;
    //     }

    //     if(damageTimer <= 0 && damageFlag){
    //         RemovePowerUp("Damage");
    //     }else if(damageTimer > 0){
    //         damageTimer -= Time.deltaTime;
    //     }
        
    // }

    void OnTriggerEnter2D(Collider2D collider){
        if(collider.tag == "Player"){
            //instantiate animation
            FindObjectOfType<Buff>().powerUpFlag = true;
            Destroy(gameObject);
        }
    }

    // void AddRandomPowerUp(){
    //     int randEff = Random.Range(1,3);
    //     Debug.Log("random broj " + randEff);
    //     switch (randEff)
    //     {
    //         case 1:
    //             FindObjectOfType<Shooting>().SetFireDelta(0.5f);
    //             attackSpeedFlag = true;
    //             attSpdTimer = powerUpDuration;
    //             break;
    //         case 2:
    //             FindObjectOfType<PlayerMovement>().SetMoveSpeed(2f);
    //             moveSpeedFlag = true;
    //             moveSpdTimer = powerUpDuration;
    //             break;
    //         case 3:
    //             FindObjectOfType<Bullet>().SetDamage(2f);
    //             damageFlag = true;
    //             damageTimer = powerUpDuration;
    //             break;
    //         default:
    //             break;
    //     }
    // }

    // void RemovePowerUp(string powUpName){
    //     switch (powUpName)
    //     {
    //         case "AttackSpeed":
    //             FindObjectOfType<Shooting>().SetToBaseFireDelta();
    //             attackSpeedFlag = false;
    //             break;
    //         case "MovementSpeed":
    //             FindObjectOfType<PlayerMovement>().SetToBaseMoveSpeed();
    //             moveSpeedFlag = false;
    //             break;
    //         case "Damage":
    //             FindObjectOfType<Bullet>().SetToBaseDamage();
    //             damageFlag = false;
    //             break;
    //         default:
    //             break;
    //     }

    // }
}
