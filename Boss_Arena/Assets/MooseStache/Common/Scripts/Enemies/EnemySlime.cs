using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MonsterLove.StateMachine;

public class EnemySlime : Actor {

	public Facings Facing;

	[Header ("Animator")]
	public Animator animator;

	// States for the state machine
	public enum States {
		Normal, 
		Dead
	}

	public LayerMask pit_layer;

	private int moveX = 0;
	private int moveY = 0;

	[Header ("Movement")]
	public int walk = 0;
	public Vector2 	direction;
	public float moveSpeed;
	public float friction;
	public Transform target;
	public int alarm = 0;

	[Header ("Death")]
	public int ScoreOnDeath = 1;

	// State Machine
	public StateMachine<States> fsm;

	new void Awake () {
		base.Awake ();
		fsm = StateMachine<States>.Initialize(this);
	}


	// Use this for initialization
	void Start () {
		alarm = 1 + (int)(Random.value * 20);
		fsm.ChangeState(States.Normal);
	}
	
	void Normal_Update () {
		if (alarm > 0) {
			alarm--;
			if (alarm <= 0) {
				behaviour ();
			}
		}
			
		if (walk > 0) {
			walk--;
			Speed = direction * moveSpeed * Time.deltaTime;
		}

		if (Speed != Vector2.zero) {

			// Turn around on pits
			var myPos = new Vector2(transform.position.x, transform.position.y);
			var targetPlace = direction * 12f;
			if (CheckColAtPlace(direction * 12f, pit_layer) || 
				!LineOfSight (myPos, myPos + targetPlace, pit_layer) || 
				CollisionSelf(pit_layer)) {
				direction *= -1f;
				Speed *= -1.4f;
			}

			// Horizontal Friction
			if (Speed.x != 0)
				Speed.x = Calc.Approach (Speed.x, 0, friction);

			// Vertical Friction
			if (Speed.y != 0)
				Speed.y = Calc.Approach (Speed.y, 0, friction);
		}
	}

	void Dead_Enter () {
		Speed = Vector2.zero;
		moveX = 0;
		moveY = 0;
	}

	void Dead_Update () {
		
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

	void behaviour () {
		if (target == null) {
			var player = GameObject.FindObjectOfType<Player> ();

			if (player != null)
				target = player.transform;
		}


		alarm = 10 + (int)(Random.value * 30);
		if (target != null) {
			var myPos = new Vector2 (transform.position.x, transform.position.y);
			var targetPos = new Vector2 (target.position.x, target.position.y);
			if (LineOfSight (myPos, targetPos, solid_layer) && LineOfSight (myPos, targetPos, pit_layer)) {
				//var degrees = Calc.Vector2ToDegree ((targetPos - myPos).normalized);
				//degrees += Random.Range (-10, 10);
				//var vector = Calc.DegreeToVector2 (degrees);
				var vector = (targetPos - myPos).normalized;
				direction = vector;
				walk = 40 + (int)(Random.value * 10);
				alarm = walk;
			} else if (Random.value < .6f) {
				var degrees = Random.value * 360f;
				var vector = Calc.DegreeToVector2 (degrees);
				direction = vector;
				walk = 10 + (int)(Random.value * 15);
				alarm = walk + 10 + (int)(Random.value * 30);
			}
		} else {
			var degrees = Random.value * 360f;
			var vector = Calc.DegreeToVector2 (degrees);
			direction = vector;
			walk = 10 + (int)(Random.value * 15);
			alarm = walk + 10 + (int)(Random.value * 30);
		}
	}

	public void Die () {
		if (ScoreManager.Instance != null) {
			ScoreManager.Instance.AddScore (ScoreOnDeath);
		}
		myCollider.enabled = false;
		fsm.ChangeState (States.Dead, StateTransition.Overwrite);
	}

	void UpdateSprite () {
		transform.localScale = new Vector3 ((int)Facing, transform.localScale.y, transform.localScale.z);

		if (fsm.State == States.Dead) {
			if (!animator.GetCurrentAnimatorStateInfo (0).IsName ("Death")) {
				animator.Play ("Death");
			}
		
		} else if (moveX == 0 && moveY == 0) {
			if (!animator.GetCurrentAnimatorStateInfo (0).IsName ("Idle")) {
				animator.Play ("Idle");
			}
		} else {
			if (!animator.GetCurrentAnimatorStateInfo (0).IsName ("Run")) {
				animator.Play ("Run");
			}
		}
	}
		
	public bool LineOfSight (Vector2 startPosition, Vector2 targetPosition, LayerMask layer) {
		var result = true;

		var hit = Physics2D.Linecast (startPosition, targetPosition, layer);

		if (hit.collider != null)
			result = false;

		return result;
	}
}
