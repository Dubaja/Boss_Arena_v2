using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour {

	[Header ("Positions & Offsets")]
	public Vector2 offsetNormal;

	[Header("Camera Recoil")]
	public float recoilForce = 10f;

	[Header ("Prefabs")]
	public Casing casingPrefab;
	public Projectile projectile;
	public GameObject muzzleFlash;

	[Header ("Fire Rate/CoolDown")]
	public float Cooldown = .1f;
	protected float cooldownTimer = 0f;

	[Header ("UI Icon")]
	public Sprite WeaponUIIcon;

	[Header ("Other")]
	public float ZOffSet = 0f;
	public GunTopDownZOrder ZOrderComponent;

	public SpriteRenderer spriteRenderer;
	public Transform spriteHolder, gunBarrel;

	public Health owner;

	protected float currentAngle = 0f;
	public float randomAngle = 20;
	
	// Update is called once per frame
	protected void Update () {
		if (cooldownTimer > 0f) {
			cooldownTimer -= Time.deltaTime;
		}
	}

	public bool TryToTriggerWeapon () {
		if (cooldownTimer > 0f) {
			return false;
		}
			
		cooldownTimer = Cooldown;
		TriggerWeapon ();

		return true;
	}

	protected void TriggerWeapon () {
		if (casingPrefab != null) {
			var c = Instantiate (casingPrefab, transform.position, Quaternion.identity);
			c.facing = (int)transform.lossyScale.x;
		}

		InstantiateMuzzleFlash ();
		InstantiateProjectile ();
	}

	public void SetRotation (float angle) {
		currentAngle = angle;

		angle += (ZOffSet);
		if (transform.lossyScale.x < 0f) {
			angle = 180 - angle;
		}
			
		//Weapon backwards or infront like in Nuclear Throne
		//if (angle > 25 && angle <= 90) {
		//	spriteRenderer.sortingOrder = -1;
		//} else {
		//	spriteRenderer.sortingOrder = 1;
		//}

		transform.localRotation = Quaternion.Euler(new Vector3(0, 0, angle));
	}

	protected virtual void InstantiateProjectile () {
		
		if (CameraShaker.instance != null) {
			CameraShaker.instance.InitShake(0.07f, 1f);
		}

		var amount = Random.Range (-randomAngle, randomAngle);
		var p = Instantiate (projectile, gunBarrel.position, Quaternion.Euler(new Vector3(0f, 0f, currentAngle + amount))) as Projectile;
		p.owner = owner;
	}

	protected void InstantiateMuzzleFlash () {
		if (muzzleFlash == null)
			return;
		var m = Instantiate (muzzleFlash, gunBarrel.position, gunBarrel.rotation);
		m.transform.localScale = new Vector3(transform.lossyScale.x, m.transform.localScale.y, m.transform.localScale.z);
	}

	public void HideWeapon () {
		spriteRenderer.enabled = false;
	}

	public void ShowWeapon () {
		spriteRenderer.enabled = true;
	}
}
