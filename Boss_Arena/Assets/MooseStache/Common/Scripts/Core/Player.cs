using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MonsterLove.StateMachine;

public class Player : Actor {

	public int playerNumber = 1;

	// Run Speed & Acceleration
	public float MaxRun = 90f; // Maximun Horizontal Run Speed
	public float RunAccel = 1000f; // Horizontal Acceleration Speed
	public float RunReduce = 400f; // Horizontal Acceleration when you're already when your horizontal speed is higher or equal to the maximun

	[Header ("Facing Direction")]
	public Facings Facing; 	// Facing Direction

	[Header ("Sprite Holder")]
	public Transform SpriteHolder;

	// Helper private Variables
	private int moveX; // Variable to store the horizontal Input each frame
	private int moveY; // Variable to store the vectical Input each frame

	[Header ("Pit")]
	public LayerMask PitLayer;
	public float FallingTime;
	private float fallingTimer;

	[Header ("Roll")]
	public float RollCooldownTime;
	private float rollCooldownTimer;
	public int RollSpeed = 160;
	private Vector2 RollDir;
	public float RollTime = 0.3f;

	public bool CanRoll
	{
		get
		{
			return input.GetButtonDown(playerNumber, InputAction.Roll) && rollCooldownTimer <= 0f;
		}
	}

	// States for the state machine
	public enum States {
		Normal, 
		Roll, 
		FallInPit,
		Dead
	}

	// State Machine
	public StateMachine<States> fsm;

	public Weapon weapon;
	public Transform weaponHolder;
	public Health health;
	public Animator Animator;
	[HideInInspector]
	public IInputManager input;

	new void Awake () {
		base.Awake ();
		fsm = StateMachine<States>.Initialize(this);
		if (health == null) {
			health = GetComponent<Health> ();
		}
	}

	// Use this for initialization
	void Start () {
		input = InputManager.instance;
		fsm.ChangeState(States.Normal);
	}
	
	// Update is called once per frame
	void Update () {
		// Update the moveX Variable and assign the current vertical input for this frame
		moveX = (int)input.GetAxis(playerNumber, InputAction.MoveX);

		// Update the moveY Variable and assign the current vertical input for this frame
		moveY = (int)input.GetAxis(playerNumber, InputAction.MoveY);

		if (rollCooldownTimer > 0f) {
			rollCooldownTimer -= Time.deltaTime;
		}	
	}
		
	void Normal_Update () {

		if (CollisionSelf(PitLayer)) {
			fsm.ChangeState(States.FallInPit, StateTransition.Overwrite);
		}

		if (CanRoll) {
			fsm.ChangeState (States.Roll, StateTransition.Overwrite);
			return;
		}

		// Movement
		Vector2 value = new Vector2(moveX, moveY);
		value.Normalize();
		Speed.x = Calc.Approach (Speed.x, value.x * MaxRun, RunAccel * Time.deltaTime);
		Speed.y = Calc.Approach (Speed.y, value.y * MaxRun, RunAccel * Time.deltaTime);

		// Aiming
		var mousePos = Camera.main.ScreenToWorldPoint (Input.mousePosition);
		//float angle = Mathf.Atan2(mousePos.y, mousePos.x) * Mathf.Rad2Deg;
		float angle = Mathf.Atan2(mousePos.y - transform.position.y, mousePos.x - transform.position.x) * 180 / Mathf.PI;

		var vectorMouse = new Vector2(mousePos.x - transform.position.x, mousePos.y - transform.position.y).normalized;
		if (PixelCameraFollower.instance != null) {
			PixelCameraFollower.instance.m_OffsetDir = vectorMouse;
		}

		if (angle < 0) {
			angle += 360f;
		}

		if (angle > 270 || angle < 90) {
			Facing = Facings.Right;
		} else {
			Facing = Facings.Left;
		}

		if (weapon != null) {
			weapon.SetRotation (angle);
		}

		// Weapon
		if ((input.GetButtonDown(playerNumber, InputAction.Fire) || input.GetButton(playerNumber, InputAction.Fire)) && weapon != null) {
			weapon.TryToTriggerWeapon ();
		}
	}

	private IEnumerator Roll_Enter () {
		if (weapon != null) {
			weapon.HideWeapon ();
		}
		rollCooldownTimer = RollCooldownTime;
		Speed = Vector2.zero;
		RollDir = Vector2.zero;

		Vector2 value = new Vector2(moveX, moveY);
		if (value == Vector2.zero) {
			value = new Vector2 ((int)Facing, 0f);
		} else if (value.x == 0 && value.y > 0 && onGround) {
			value = new Vector2 ((int)Facing, value.y);
		}
		value.Normalize();
		Vector2 vector = value * RollSpeed;
		Speed = vector;
		RollDir = value;
		if (RollDir.x != 0f) {
			Facing = (Facings)Mathf.Sign (RollDir.x);
		}

		// Screenshake
		if (CameraShaker.instance != null) {
			CameraShaker.instance.InitShake(0.125f, 1f);
		}

		// Invincibility
		health.invincible = true;

		yield return new WaitForSeconds (RollTime);

		// Wait one extra frame
		yield return null;

		fsm.ChangeState (States.Normal, StateTransition.Overwrite);
		yield break;
	}

	void Roll_Exit () {
		if (weapon != null) {
			weapon.ShowWeapon ();
		}

		// Reset Invincibility
		health.invincible = false;
	}

	void FallInPit_Enter () {
		// Set the falling timer
		fallingTimer = FallingTime;

		// Disable collider and reset speed
		myCollider.enabled = false;
		Speed.x = 0f;
		Speed.y = 0f;
		moveX = 0;
		moveY = 0;

		if (weapon != null) {
			weapon.HideWeapon ();
		}
	}

	void FallInPit_Update () {
		fallingTimer -= Time.deltaTime;

		// Die if the time has passed
		if (fallingTimer <= 0) {
			Die ();
		}
	}

	void LateUpdate () {
		// Horizontal Movement
		var moveh = base.MoveH (Speed.x * Time.deltaTime);
		if (moveh)
			Speed.x = 0;

		// Vertical Movement
		var movev = base.MoveV (Speed.y * Time.deltaTime);
		if (movev)
			Speed.y = 0;

		UpdateSprite ();
	}

	void UpdateSprite () {
		var targetScale = Facing == Facings.Right ? new Vector3(1f,1f,1f) : new Vector3(-1f,1f,1f);
		transform.localScale = targetScale;

		if (fsm.State == States.Dead) {
			if (!Animator.GetCurrentAnimatorStateInfo (0).IsName ("Dead")) {
				Animator.Play ("Dead");
			}
		} else if (fsm.State == States.FallInPit) {
			if (!Animator.GetCurrentAnimatorStateInfo (0).IsName ("Fall")) {
				Animator.Play ("Fall");
			}
		} else if (fsm.State == States.Roll) {
			if (!Animator.GetCurrentAnimatorStateInfo (0).IsName ("Roll")) {
				Animator.Play ("Roll");
			}
		} else if (moveX == 0 && moveY == 0) {
			if (!Animator.GetCurrentAnimatorStateInfo (0).IsName ("Idle") && 
				!Animator.GetCurrentAnimatorStateInfo (0).IsName ("TakeHit")) {
				Animator.Play ("Idle");
			}
		} else {
			if (!Animator.GetCurrentAnimatorStateInfo (0).IsName ("Run") &&
				!Animator.GetCurrentAnimatorStateInfo (0).IsName ("TakeHit")) {
				Animator.Play ("Run");
			}
		}
	}

	public void UnEquipWeapon () {
		if (weapon != null) {
			Destroy (weapon.gameObject);
		}
	}

	public void EquipWeapon (Weapon wep) {
		if (weapon != null) {
			UnEquipWeapon ();
		}

		weapon = Instantiate (wep, new Vector2(weaponHolder.position.x, weaponHolder.position.y) + wep.offsetNormal, Quaternion.identity, weaponHolder) as Weapon;
		weapon.owner = GetComponent<Health> ();
		if (weapon.ZOrderComponent != null) {
			weapon.ZOrderComponent.targetSpriteRenderer = SpriteHolder.GetComponent<SpriteRenderer> ();
		}

		if (UIWeaponIcon.instance != null) {
			UIWeaponIcon.instance.SetWeaponIcon (weapon.WeaponUIIcon);
		}
	}

	public void SetTakeHitAnim () {
		if (fsm.State != States.Normal)
			return;
		
		if (!Animator.GetCurrentAnimatorStateInfo (0).IsName ("TakeHit")) {
			Animator.Play ("TakeHit");
		}

		if (CameraShaker.instance != null) {
			CameraShaker.instance.InitShake(0.075f, 2f);
		}
	}	

	public void Die () {
		if (CameraShaker.instance != null) {
			CameraShaker.instance.InitShake(0.25f, 1f);
		}

		fsm.ChangeState (States.Dead, StateTransition.Overwrite);
		myCollider.enabled = false;
		Speed = Vector2.zero;

		GameManager.instance.InvokeRestartScene (2f);
	}

}
