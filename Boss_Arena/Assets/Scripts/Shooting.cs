using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shooting : MonoBehaviour
{
	public Transform firePoint;
	public GameObject bulletPrefab;

    private float baseFireDelta = 0.3f;
    private float baseDamage = 20f;
	public float damage;

	public float fireDelta = 0.5f;
	private float myTime = 0.0f;
	private float nextFire = 0.5f;

	public float bulletForce = 2f;

    void Start(){
        damage = baseDamage;
        fireDelta = baseFireDelta;
    }


    // Update is called once per frame
    void Update()
    {
		myTime = myTime + Time.deltaTime;

        if (Input.GetButton("Fire1") && myTime > nextFire)
        {
            nextFire = myTime + fireDelta;
            Shoot();

            nextFire = nextFire - myTime;
            myTime = 0.0F;
        }
    }

    void Shoot(){
    	GameObject bullet = Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);
    	Rigidbody2D rb = bullet.GetComponent<Rigidbody2D>();
    	rb.AddForce(firePoint.up * bulletForce, ForceMode2D.Impulse);
    	Destroy(bullet, 2f);
    }

    public void SetFireDelta(float factor){
        fireDelta *= factor;
    }

    public void SetToBaseFireDelta(){
        fireDelta = baseFireDelta;
    }

    public void SetDamage(float factor){
		damage *= factor;
	}

	public void SetToBaseDamage(){
		damage = baseDamage;
	}
}
