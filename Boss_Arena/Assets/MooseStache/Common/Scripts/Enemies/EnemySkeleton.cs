using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MonsterLove.StateMachine;

public class EnemySkeleton : Actor {

	public Facings Facing;
	public Weapon weapon;

	[Header ("Animator")]
	public Animator animator;

	// States for the state machine
	public enum States {
		Spawn,
		Normal, 
		Dead
	}

	public LayerMask pit_layer;

	private float spawnTimer = 1.5f;

	[Header ("Movement")]
	public int walk = 0;
	public Vector2 	direction;
	public float moveSpeed;
	public float friction;
	public Transform target;
	public int alarm = 0;

	[Header ("Death")]
	public int ScoreOnDeath = 3;

	// State Machine
	public StateMachine<States> fsm;

	new void Awake () {
		base.Awake ();
		fsm = StateMachine<States>.Initialize(this);
	}


	// Use this for initialization
	void Start () {
		alarm = 1 + (int)(Random.value * 20);
		fsm.ChangeState(States.Spawn);
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

		if (fsm.State == States.Spawn) {
			if (!animator.GetCurrentAnimatorStateInfo (0).IsName ("Spawn")) {
				animator.Play ("Spawn");
			}
		} else if (fsm.State == States.Dead) {
			if (!animator.GetCurrentAnimatorStateInfo (0).IsName ("Death")) {
				animator.Play ("Death");
			}
		} else if (Speed == Vector2.zero) {
			if (!animator.GetCurrentAnimatorStateInfo (0).IsName ("Idle")) {
				animator.Play ("Idle");
			}
		} else {
			if (!animator.GetCurrentAnimatorStateInfo (0).IsName ("Run")) {
				animator.Play ("Run");
			}
		}
	}

	void Spawn_Enter () {
		// just the time the spawn animation lasts for
		spawnTimer = 1.5f;
	}	

	void Spawn_Update () {
		if (spawnTimer > 0f) {
			spawnTimer -= Time.deltaTime;

			if (spawnTimer <= 0f) {
				fsm.ChangeState (States.Normal);
			}
		}	
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

	void behaviour () {
		if (target == null) {
			var player = GameObject.FindObjectOfType<Player> ();

			if (player != null)
				target = player.transform;
		}

		//var degrees = Calc.Vector2ToDegree ((targetPos - myPos).normalized);
		//degrees += Random.Range (-10, 10);
		//var vector = Calc.DegreeToVector2 (degrees);

		alarm = 20 + (int)(Random.value * 10);
		if (target != null) {
			var myPos = new Vector2 (transform.position.x, transform.position.y);
			var targetPos = new Vector2 (target.position.x, target.position.y);
			if (LineOfSight (myPos, targetPos, solid_layer) && LineOfSight (myPos, targetPos, pit_layer)) {
				if (Vector2.Distance (myPos, targetPos) > 24) {
					if (Random.value < 0.45f) {
						PointTowardsPlayer ();
						weapon.TryToTriggerWeapon ();
						alarm = 20 + (int)(Random.value * 5);
					} else {
						var degrees = Calc.Vector2ToDegree ((targetPos - myPos).normalized);
						degrees += Random.Range (-90, 90);
						var vector = Calc.DegreeToVector2 (degrees);
						direction = vector;
						walk = 10 + (int)(Random.value * 10);
						PointTowardsPlayer ();
					}
				} else {
					var degrees = Calc.Vector2ToDegree ((myPos - targetPos).normalized);
					degrees += Random.Range (-10, 10);
					var vector = Calc.DegreeToVector2 (degrees);
					direction = vector;
					walk = 40 + (int)(Random.value * 10);
					PointTowardsPlayer ();
				}

				if (targetPos.x < myPos.x) {
					Facing = Facings.Left;
				} else if (targetPos.x > myPos.x) {
					Facing = Facings.Right;
				}
					
			} else if (Random.value < 0.25f) {
				var degrees = Random.value * 360f;
				var vector = Calc.DegreeToVector2 (degrees);
				direction = vector;
				walk = 20 + (int)(Random.value * 10);
				alarm = walk + 10 + (int)(Random.value * 30);
				PointTowardsDirection (vector);

				if (Speed.x < 0) {
					Facing = Facings.Left;
				} else if (Speed.x > 0) {
					Facing = Facings.Right;
				}
			}

		} else if (Random.value < 0.1f) {
			var degrees = Random.value * 360f;
			var vector = Calc.DegreeToVector2 (degrees);
			direction = vector;
			walk = 20 + (int)(Random.value * 10);
			alarm = walk + 10 + (int)(Random.value * 30);
			PointTowardsDirection (vector);

			if (Speed.x < 0) {
				Facing = Facings.Left;
			} else if (Speed.x > 0) {
				Facing = Facings.Right;
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

	public void PointTowardsPlayer () {
		var myPos = new Vector2 (transform.position.x, transform.position.y);
		var targetPos = new Vector2 (target.position.x, target.position.y);
		var vector = (targetPos - myPos).normalized;
		var angle = Mathf.Atan2 (targetPos.y - myPos.y, targetPos.x - myPos.x) * 180 / Mathf.PI;
		if (angle < 0) {
			angle += 360f;
		}
		weapon.SetRotation (angle);
	}

	public void PointTowardsDirection (Vector2 dir) {
		var myPos = new Vector2 (transform.position.x, transform.position.y);
		var targetPos = new Vector2 (myPos.x + (dir.x * 999), myPos.y * (dir.y * 999));
		var vector = (targetPos - myPos).normalized;
		var angle = Mathf.Atan2 (targetPos.y - myPos.y, targetPos.x - myPos.x) * 180 / Mathf.PI;
		if (angle < 0) {
			angle += 360f;
		}
		weapon.SetRotation (angle);
	}

	public void Die () {
		if (ScoreManager.Instance != null) {
			ScoreManager.Instance.AddScore (ScoreOnDeath);
		}
		myCollider.enabled = false;
		fsm.ChangeState (States.Dead, StateTransition.Overwrite);
	}
		
}
