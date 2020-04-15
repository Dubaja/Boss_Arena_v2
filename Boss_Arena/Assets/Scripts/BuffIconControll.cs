using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuffIconControll : MonoBehaviour
{  
    public GameObject redIcon;
    public GameObject greenIcon;
    public GameObject blueIcon;
    // Start is called before the first frame update
    void Start()
    {
        redIcon.SetActive(false);
        greenIcon.SetActive(false);
        blueIcon.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if(GameObject.Find("Player")){
            if(FindObjectOfType<Buff>().GetAttackSpeedFlag()){
                greenIcon.SetActive(true);
            }else{
                greenIcon.SetActive(false);
            }

            if(FindObjectOfType<Buff>().GetMoveSpeedFlag()){
                blueIcon.SetActive(true);
            }else{
                blueIcon.SetActive(false);
            }

            if(FindObjectOfType<Buff>().GetDamageFlag()){
                redIcon.SetActive(true);
            }else{
                redIcon.SetActive(false);
            }
        }
    }
}
