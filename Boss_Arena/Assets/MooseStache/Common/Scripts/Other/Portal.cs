using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MonsterLove.StateMachine;

public class Portal : MonoBehaviour {

	// States for the state machine
	public enum States {
		WaitToActivate,
		Activating,
		Deactivating,
		Idle, 
		SpawningSlime
	}

	public float waitTime = 1f;

	[Header ("Activating")]
	public float ActivatingTime = 2.1f;
	private float activatingTimer = 0f;

	[Header ("Deactivating")]
	public float DeactivatingTime = 2.1f;
	public float TimeBeforeDeactivating = 1f;
	private float timeBeforeDeactivatingTimer = 0f;

	[Header ("Idle")]
	public float TimeBetweenSpawns = 1f;
	private float timeBetweenSpawnsTimer = 0f;

	[Header ("Spawn Slime")]
	public int spawnAmount = 1;
	public float slimeSpawnTime = 1.1f;


	public Animator animator;

	public GameObject SlimePrefab;
	public Vector2 SlimeOffSet;


	// State Machine
	public StateMachine<States> fsm;

	void Awake () {
		fsm = StateMachine<States>.Initialize(this);
	}

	void Update () {
		UpdateSprite ();
	}

	// Use this for initialization
	void Start () {
		fsm.ChangeState (States.WaitToActivate);
		Invoke ("EnterActivating", waitTime);
	}

	void EnterActivating () {
		fsm.ChangeState(States.Activating);
	}

	void Activating_Enter () {
		activatingTimer = ActivatingTime;
	}

	void Activating_Update () {
		if (activatingTimer > 0f) {
			activatingTimer -= Time.deltaTime;

			if (activatingTimer <= 0f) {
				fsm.ChangeState (States.Idle, StateTransition.Overwrite);
			}
		}
	}

	void Idle_Enter () {
		if (spawnAmount > 0) {
			spawnAmount--;
			timeBetweenSpawnsTimer = TimeBetweenSpawns;
		} else {
			
			timeBeforeDeactivatingTimer = TimeBeforeDeactivating;
		}
	}

	void Idle_Update () {
		if (timeBetweenSpawnsTimer > 0f) {
			timeBetweenSpawnsTimer -= Time.deltaTime;

			if (timeBetweenSpawnsTimer <= 0f) {
				SpawnSlime ();
			}
		}

		if (timeBeforeDeactivatingTimer > 0f) {
			timeBeforeDeactivatingTimer -= Time.deltaTime;

			if (timeBeforeDeactivatingTimer <= 0f) {
				fsm.ChangeState (States.Deactivating, StateTransition.Overwrite);
			}
		}
	}

	IEnumerator SpawningSlime_Enter () {
		yield return new WaitForSeconds (slimeSpawnTime);

		Instantiate (SlimePrefab, new Vector2(transform.position.x, transform.position.y) + SlimeOffSet, Quaternion.identity);

		fsm.ChangeState (States.Idle, StateTransition.Overwrite);
	}

	IEnumerator Deactivating_Enter () {
		yield return new WaitForSeconds (DeactivatingTime);

		Destroy (gameObject);
	}

	void SpawnSlime () {
		fsm.ChangeState (States.SpawningSlime, StateTransition.Overwrite);
	}

	void UpdateSprite () {

		if (fsm.State == States.Activating) {
			if (!animator.GetCurrentAnimatorStateInfo (0).IsName ("Activate")) {
				animator.Play ("Activate");
			}
		} else if (fsm.State == States.Idle) {
			if (!animator.GetCurrentAnimatorStateInfo (0).IsName ("Idle")) {
				animator.Play ("Idle");
			}
		} else if (fsm.State == States.SpawningSlime) {
			if (!animator.GetCurrentAnimatorStateInfo (0).IsName ("SpawnSlime")) {
				animator.Play ("SpawnSlime");
			}
		} else if (fsm.State == States.Deactivating) {
			if (!animator.GetCurrentAnimatorStateInfo (0).IsName ("Deactivate")) {
				animator.Play ("Deactivate");
			}
		} 

	}
}
