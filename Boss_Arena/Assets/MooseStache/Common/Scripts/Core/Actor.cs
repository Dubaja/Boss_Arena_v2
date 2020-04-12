using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(BoxCollider2D))]
public class Actor : MonoBehaviour {

	[Header ("Variabless")]
	public bool onGround = true; // Variable to store wether or not the actor is on the ground on the current frame
	public bool wasOnGround = true; // Same as the onGround but stores the frame before the current one (last)
	//public bool canPushBlock = false;

	[Header ("Speed & SubPixel Movement Counter")]
	public Vector2 Speed; // The Speed of the Actor
	public Vector2 movementCounter = Vector2.zero;  // Counter for subpixel movement

	[Header ("Collision Layers")]
	public LayerMask solid_layer; // The layer on which solids are placed
	public LayerMask entities_layer; // The layer on which the OneWay/FallThrough platforms are
	//public LayerMask pushblock_layer;
	[SerializeField, Header("Collider")]
	protected Collider2D myCollider; // Cached collider (only use Collider2Ds)

	protected void Awake () {
		// If the collider has not been assigned manually we'll try to get the collider2d component of this object
		if (myCollider == null) {
			myCollider = GetComponent<Collider2D> ();
			if (myCollider == null) {
				Debug.Log ("This Actor has no Collider2D component");
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

	// Function to move the Actor Vertically, this only stores the float value of the movement to allow for subpixel movement and calls the MoveVExact function to do the actual movement
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
	public bool MoveHExact(int moveH) {
		int num = (int)Mathf.Sign((float)moveH);
		while (moveH != 0) {
			bool solid = CheckColInDir(Vector2.right * (float)num, solid_layer);
			if (solid) {
				this.movementCounter.x = 0f;
				return true;
			}
			moveH -= num;
			transform.position = new Vector2 (transform.position.x + (float)num, transform.position.y);
		}
		return false;
	}

	// Function to move the Actor Vertically an exact integer amount
	public bool MoveVExact(int moveV) {
		int num = (int)Mathf.Sign((float)moveV);
		while (moveV != 0) {
			bool solid = num > 0 ? CheckColInDir(Vector2.up * (float)num, solid_layer) : OnGround();
			if (solid) {
				this.movementCounter.y = 0f;
				return true;
			}
			moveV -= num;
			transform.position = new Vector2 (transform.position.x, transform.position.y + (float)num);
		}
		return false;
	}

	// Checks wether the actor is grounded or not 
	public bool OnGround () {
		// checking if there is a collision on the bottom with the solid layer or if there is a collision with the oneway layer and you are not already collisioning with it
		return CheckColInDir (Vector2.down, solid_layer);
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

	// Checks if there is a collision on top of the actor in a given layer (specially good to check if your are on top of a oneway/fallthrough platform or going through it)
	public bool CollisionSelf (LayerMask layer) {
		Vector2 leftcorner = new Vector2 (myCollider.bounds.center.x - myCollider.bounds.extents.x + .1f, myCollider.bounds.center.y + myCollider.bounds.extents.y - .1f);
		Vector2 rightcorner = new Vector2 (myCollider.bounds.center.x + myCollider.bounds.extents.x - .1f, myCollider.bounds.center.y - myCollider.bounds.extents.y + .1f);
		return Physics2D.OverlapArea(leftcorner, rightcorner, layer);
	}

	// Helper function to check if there is any collision within a given layer with an extra set position
	public bool CheckColAtPlace (Vector2 extraPos, LayerMask layer) {
		Vector2 leftcorner = Vector2.zero; 
		Vector2 rightcorner = Vector2.zero;

		leftcorner = new Vector2 (myCollider.bounds.center.x - myCollider.bounds.extents.x + .1f, myCollider.bounds.center.y + myCollider.bounds.extents.y - .1f) + extraPos;
		rightcorner = new Vector2 (myCollider.bounds.center.x + myCollider.bounds.extents.x - .1f, myCollider.bounds.center.y - myCollider.bounds.extents.y + .1f) + extraPos;

		return Physics2D.OverlapArea(leftcorner, rightcorner, layer);
	}
}
