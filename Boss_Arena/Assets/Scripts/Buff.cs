using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Buff : MonoBehaviour
{
    public bool powerUpFlag;
    private float powerUpDuration = 15f;
    private bool attackSpeedFlag;
    private bool moveSpeedFlag;
    private bool damageFlag;
    private float attSpdTimer;
    private float moveSpdTimer;
    private float damageTimer;
    private int randMax = 3;

    public GameObject pUp;

    void Start(){
        powerUpFlag = false;
        attackSpeedFlag = false;
        moveSpeedFlag = false;
        damageFlag = false;
    }

    void Update(){
        if(powerUpFlag){
            AddRandomPowerUp();
            powerUpFlag = false;
        }

        if(attSpdTimer <= 0 && attackSpeedFlag){
            RemovePowerUp("AttackSpeed");
        }else if(attSpdTimer > 0){
            attSpdTimer -= Time.deltaTime;
        }

        if(moveSpdTimer <= 0 && moveSpeedFlag){
            RemovePowerUp("MovementSpeed");
        }else if(moveSpdTimer > 0){
            moveSpdTimer -= Time.deltaTime;
        }

        if(damageTimer <= 0 && damageFlag){
            RemovePowerUp("Damage");
        }else if(damageTimer > 0){
            damageTimer -= Time.deltaTime;
        }
        
    }

    void AddRandomPowerUp(){
        int randEff = Random.Range(1,randMax + 1);
        Debug.Log("random broj " + randEff);
        switch (randEff)
        {
            case 1:
                if(!attackSpeedFlag){
                    FindObjectOfType<Shooting>().SetFireDelta(0.5f);
                }
                attackSpeedFlag = true;
                attSpdTimer = powerUpDuration;
                break;
            case 2:
                if(!moveSpeedFlag){
                    FindObjectOfType<PlayerMovement>().SetMoveSpeed(2f);
                }
                moveSpeedFlag = true;
                moveSpdTimer = powerUpDuration;
                break;
            case 3:
                if(!damageFlag){
                    FindObjectOfType<Shooting>().SetDamage(2f);
                }
                damageFlag = true;
                damageTimer = powerUpDuration;
                break;
            default:
                break;
        }
    }

    void RemovePowerUp(string powUpName){
        switch (powUpName)
        {
            case "AttackSpeed":
                FindObjectOfType<Shooting>().SetToBaseFireDelta();
                attackSpeedFlag = false;
                break;
            case "MovementSpeed":
                FindObjectOfType<PlayerMovement>().SetToBaseMoveSpeed();
                moveSpeedFlag = false;
                break;
            case "Damage":
                FindObjectOfType<Shooting>().SetToBaseDamage();
                damageFlag = false;
                break;
            default:
                break;
        }

    }

    public bool GetMoveSpeedFlag(){
        return moveSpeedFlag;
    }
    public bool GetAttackSpeedFlag(){
        return attackSpeedFlag;
    }
    public bool GetDamageFlag(){
        return damageFlag;
    }
}
