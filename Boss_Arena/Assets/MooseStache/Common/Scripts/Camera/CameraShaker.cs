using System;
using UnityEngine;

public class CameraShaker : MonoBehaviour
{
	public static CameraShaker instance;

	private Vector3 lastRealPos;

	private Transform trans;

	private Vector3 defPos = new Vector3(0f, 0f, 0f);

	private float shakeTime;

	private float shakePwr;

	private void Awake()
	{
		CameraShaker.instance = this;
	}

	private void Start()
	{
		trans = base.transform;
	}

	private void Update()
	{
		if (shakeTime > 0f)
		{
			var value = UnityEngine.Random.insideUnitCircle.normalized * shakePwr;
			trans.localPosition = defPos + new Vector3(value.x, value.y, 0f);
			shakeTime -= Time.deltaTime;
			if (shakeTime <= 0f) {
				shakePwr = 0f;
				trans.localPosition = defPos;
			}
		}
	}

	public void InitShake(float time, float pwr)
	{
		if (pwr >= shakePwr && time >= shakeTime) {
			shakeTime = time;
			shakePwr = pwr;
		}
	}

}
