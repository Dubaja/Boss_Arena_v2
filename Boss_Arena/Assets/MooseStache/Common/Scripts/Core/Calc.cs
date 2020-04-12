using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Calc {

	public static float Approach (float Start, float End, float Shift) {
		if (Start < End)
			return Mathf.Min (Start + Shift, End);
		else
			return Mathf.Max(Start - Shift, End);
	}

	public static Vector2 RadianToVector2(float radian)
	{
		return new Vector2(Mathf.Cos(radian), Mathf.Sin(radian));
	}

	public static Vector2 DegreeToVector2(float degree)
	{
		return RadianToVector2(degree * Mathf.Deg2Rad);
	}

	public static float Vector2ToDegree (Vector2 angleVector) {
		//return (float)Mathf.Atan2(angleVector.x, -angleVector.y);

		float value = (float)((Mathf.Atan2(angleVector.x, angleVector.y) / Mathf.PI) * 180f);
		if(value < 0) value += 360f;

		return value;

	}
}
