using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//[RequireComponent (typeof(Rigidbody2D))]
public class Projectile : MonoBehaviour {

	[Header ("Speed")]
	public float baseSpeed;
	public float randomSpeed;
	public Vector2 SpeedV2;
	public Vector2 Direction;

	[Header ("Damage")]
	public int DamageOnHit;

	[Header ("Layers")]
	public LayerMask solid_layer;
	public LayerMask entities_layer;

	[Header ("OnHit FX")]
	public GameObject HitFxPrefab;
	public GameObject DustFxPrefab;

	[Header ("Bounce")]
	public bool BounceOnCollide = false;
	public int bouncesLeft = 0;

	[HideInInspector]
	public Health owner; // owner of the projectile
	private Vector2 Position; // Current position
	private Vector2 movementCounter = Vector2.zero;  // Counter for subpixel movement
	public BoxCollider2D myCollider; 
	List<Health> healthsDamaged = new List<Health>(); // List to store healths damaged

	void Awake () {
		if (myCollider == null) {
			myCollider = GetComponent<BoxCollider2D> ();
		}
	}

	void Start () {
		// keeping everything Pixel perfect
		Position = new Vector2 (Mathf.Round(transform.position.x), Mathf.Round(transform.position.y));
		transform.position = Position;
	}

	void Update () {
		SpeedV2 = new Vector2 (transform.right.x, transform.right.y) * (baseSpeed + Random.value * randomSpeed) * Time.deltaTime;
	}

	void LateUpdate () {
		if (SpeedV2.x != 0) {
			MoveH (SpeedV2.x);
		}

		if (SpeedV2.y != 0) {
			MoveV (SpeedV2.y);
		}
	}

	void DestroyMe () {
		if (HitFxPrefab != null) {
			var h = Instantiate (HitFxPrefab, transform.position, transform.rotation);
			h.transform.localScale = transform.lossyScale;
			h.transform.localRotation = Quaternion.Euler (new Vector3(0f, 0f, Random.value * 360f));
		}
		Destroy (gameObject);
	}

	void DestroyMeWall () {
		if (HitFxPrefab != null) {
			var h = Instantiate (HitFxPrefab, transform.position, transform.rotation);
			h.transform.localScale = transform.lossyScale;
			h.transform.localRotation = Quaternion.Euler (new Vector3(0f, 0f, Random.value * 360f));
		}
		Destroy (gameObject);
	}

	public void BounceHorizontal () {
		bouncesLeft--;
		transform.right = new Vector3 (-transform.right.x, transform.right.y, transform.right.z);
		SpeedV2 *= 0.8f;
	}

	public void BounceVertical () {
		bouncesLeft--;
		transform.right = new Vector3 (transform.right.x, -transform.right.y, transform.right.z);
		SpeedV2 *= 0.8f;
	}
			
	void OnCollideWith (Collider2D col, bool horizontalCol = true) {
		var component = col.GetComponent<Health> ();
		// If the target the hitbox collided with has a health component and it is not our owner and it is not on the already on the list of healths damaged by the current hitbox
		if (component != null && component != owner && !healthsDamaged.Contains(component)) {
			// Add the health component to the list of damaged healths
			healthsDamaged.Add (component);

			// Apply the damage
			var didDamage = component.TakeDamage (DamageOnHit);
			// Destroy the projectile after applying damage
			if (didDamage) {
				DestroyMe ();
				return;
			}
		}
			
		// if the projectile hit's a solid object, destroy it
		if (col.gameObject.layer ==  (int)Mathf.Log(solid_layer.value, 2)) {
			DestroyMeWall ();
			return;
		}
	}

	void OnCollideWithEntity(Collider2D col) {
		var component = col.GetComponent<Health> ();
		// If the target the hitbox collided with has a health component and it is not our owner and it is not on the already on the list of healths damaged by the current hitbox
		if (component != null && component != owner && !healthsDamaged.Contains(component)) {
			// Add the health component to the list of damaged healths
			healthsDamaged.Add (component);

			// Apply the damage
			var didDamage = component.TakeDamage (DamageOnHit);
			// Destroy the projectile after applying damage
			if (didDamage) {
				DestroyMe ();
			}
		}
	}


	// Function to move the Actor Horizontally, this only stores the float value of the movement to allow for subpixel movement and calls the MoveHExact function to do the actual movement
	public bool MoveH(float moveH) {
		this.movementCounter.x = this.movementCounter.x + moveH;
		int num = (int)Mathf.Round(this.movementCounter.x);
		if (num != 0)
		{
			this.movementCounter.x = this.movementCounter.x - (float)num;
			return this.MoveHExact(num);
		}
		return false;
	}

	// Function to move the Actor Horizontally, this only stores the float value of the movement to allow for subpixel movement and calls the MoveHExact function to do the actual movement
	public bool MoveV(float moveV) {
		this.movementCounter.y = this.movementCounter.y + moveV;
		int num = (int)Mathf.Round(this.movementCounter.y);
		if (num != 0)
		{
			this.movementCounter.y = this.movementCounter.y - (float)num;
			return this.MoveVExact(num);
		}
		return false;
	}

	// Function to move the Actor Horizontally an exact integer amount
	public bool MoveVExact(int moveV) {
		int num = (int)Mathf.Sign((float)moveV);
		while (moveV != 0) {
			bool solid = CheckColInDir(Vector2.up * (float)num, solid_layer);
			if (solid) {
				if (BounceOnCollide && bouncesLeft > 0) {
					bouncesLeft--;
					num = -num;
					moveV = -moveV;
					BounceVertical ();
				} else {
					this.movementCounter.x = 0f;
					DestroyMeWall ();
					return true;
				}
			}

			bool entity = CheckColInDir(Vector2.up * (float)num, entities_layer);
			if (entity) {
				var entit = CheckColsInDirAll (Vector2.up * (float)num, entities_layer);
				OnCollideWithEntity (entit [0]);
			}
	
			moveV -= num;
			transform.position = new Vector2 (transform.position.x, transform.position.y + (float)num);
		}
		return false;
	}


	// Function to move the Actor Horizontally an exact integer amount
	public bool MoveHExact(int moveH) {
		int num = (int)Mathf.Sign((float)moveH);
		while (moveH != 0) {
			bool solid = CheckColInDir(Vector2.right * (float)num, solid_layer);
			if (solid) {
				if (BounceOnCollide && bouncesLeft > 0) {
					bouncesLeft--;
					num = -num;
					moveH = -moveH;
					BounceHorizontal ();
				} else {
					this.movementCounter.x = 0f;
					DestroyMeWall ();
					return true;
				}
			}

			bool entity = CheckColInDir(Vector2.right * (float)num, entities_layer);
			if (entity) {
				var entit = CheckColsInDirAll (Vector2.right * (float)num, entities_layer);
				OnCollideWithEntity (entit [0]);
			}

			moveH -= num;
			transform.position = new Vector2 (transform.position.x + (float)num, transform.position.y);
		}
		return false;
	}

	// Helper function to check if there is any collision within a given layer in a set direction (only use up, down, left, right)
	public bool CheckColInDir (Vector2 dir, LayerMask layer) {
		Vector2 leftcorner = Vector2.zero; 
		Vector2 rightcorner = Vector2.zero;

		if (dir.x > 0) {
			leftcorner = new Vector2 (myCollider.bounds.center.x + myCollider.bounds.extents.x, myCollider.bounds.center.y + myCollider.bounds.extents.y - .1f);
			rightcorner = new Vector2 (myCollider.bounds.center.x + myCollider.bounds.extents.x + .5f, myCollider.bounds.center.y - myCollider.bounds.extents.y + .1f);
		} else if (dir.x < 0) {
			leftcorner = new Vector2 (myCollider.bounds.center.x - myCollider.bounds.extents.x - .5f, myCollider.bounds.center.y + myCollider.bounds.extents.y - .1f);
			rightcorner = new Vector2 (myCollider.bounds.center.x - myCollider.bounds.extents.x, myCollider.bounds.center.y - myCollider.bounds.extents.y + .1f);
		} else if (dir.y > 0) {
			leftcorner = new Vector2 (myCollider.bounds.center.x - myCollider.bounds.extents.x + .1f, myCollider.bounds.center.y + myCollider.bounds.extents.y + .5f);
			rightcorner = new Vector2 (myCollider.bounds.center.x + myCollider.bounds.extents.x - .1f, myCollider.bounds.center.y + myCollider.bounds.extents.y);
		} else if (dir.y < 0) {
			leftcorner = new Vector2 (myCollider.bounds.center.x - myCollider.bounds.extents.x + .1f, myCollider.bounds.center.y - myCollider.bounds.extents.y);
			rightcorner = new Vector2 (myCollider.bounds.center.x + myCollider.bounds.extents.x - .1f, myCollider.bounds.center.y - myCollider.bounds.extents.y - .5f);
		}

		return Physics2D.OverlapArea(leftcorner, rightcorner, layer);
	}
		

	// The same as CheckColInDir but it returns a Collider2D array of the colliders you're collisioning with
	public Collider2D[] CheckColsInDirAll (Vector2 dir, LayerMask layer) {
		Vector2 leftcorner = Vector2.zero; 
		Vector2 rightcorner = Vector2.zero;

		if (dir.x > 0) {
			leftcorner = new Vector2 (myCollider.bounds.center.x + myCollider.bounds.extents.x, myCollider.bounds.center.y + myCollider.bounds.extents.y - .1f);
			rightcorner = new Vector2 (myCollider.bounds.center.x + myCollider.bounds.extents.x + .5f, myCollider.bounds.center.y - myCollider.bounds.extents.y + .1f);
		} else if (dir.x < 0) {
			leftcorner = new Vector2 (myCollider.bounds.center.x - myCollider.bounds.extents.x - .5f, myCollider.bounds.center.y + myCollider.bounds.extents.y - .1f);
			rightcorner = new Vector2 (myCollider.bounds.center.x - myCollider.bounds.extents.x, myCollider.bounds.center.y - myCollider.bounds.extents.y + .1f);
		} else if (dir.y > 0) {
			leftcorner = new Vector2 (myCollider.bounds.center.x - myCollider.bounds.extents.x + .1f, myCollider.bounds.center.y + myCollider.bounds.extents.y + .5f);
			rightcorner = new Vector2 (myCollider.bounds.center.x + myCollider.bounds.extents.x - .1f, myCollider.bounds.center.y + myCollider.bounds.extents.y);
		} else if (dir.y < 0) {
			leftcorner = new Vector2 (myCollider.bounds.center.x - myCollider.bounds.extents.x + .1f, myCollider.bounds.center.y - myCollider.bounds.extents.y);
			rightcorner = new Vector2 (myCollider.bounds.center.x + myCollider.bounds.extents.x - .1f, myCollider.bounds.center.y - myCollider.bounds.extents.y - .5f);
		}

		return Physics2D.OverlapAreaAll(leftcorner, rightcorner, layer);
	}
}

