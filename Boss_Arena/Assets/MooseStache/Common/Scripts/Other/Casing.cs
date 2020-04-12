using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Casing : Actor {

	public float speed = 100f;
	public float friction = 1f;
	public Vector2 direction;
	public int facing = 1;

	public float minSpeed = 50f;
	public float maxSpeed = 100f;

	//public int fadeFrames = 40;

	void Start () {
		transform.localRotation = Quaternion.Euler(new Vector3 (0f, 0f, Random.value * 360f));

		direction = Calc.DegreeToVector2(Random.Range (105f, 165f));
		//Debug.Log (direction.ToString ());
		direction.x *= facing;

		speed = Random.Range (minSpeed, maxSpeed);
		Speed = speed * direction;
	}

	void Update () {
		/*
		if (fadeFrames > 0) {
			fadeFrames--;

			if (fadeFrames <= 0) {
				Destroy (gameObject);
			}
		}
		*/
		if (Speed != Vector2.zero) {
			if (Speed.x != 0)
				Speed.x = Calc.Approach (Speed.x, 0, friction * Time.deltaTime);

			if (Speed.y != 0)
				Speed.y = Calc.Approach (Speed.y, 0, friction * Time.deltaTime);
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
	}
}
