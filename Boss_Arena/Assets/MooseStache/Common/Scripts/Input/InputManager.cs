using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class InputManager : MonoBehaviour, IInputManager {

	private static InputManager _instance;

	public static IInputManager instance { get { return _instance; } }

	public static void SetInstance (InputManager instance) {
		if (InputManager._instance == instance)
			return;

		if (InputManager._instance != null) {
			InputManager._instance.enabled = false;
		}

		InputManager._instance = instance;
	}

	private bool _dontDestroyOnLoad = true;

	protected virtual void Awake () {
		if (_dontDestroyOnLoad) DontDestroyOnLoad (this.transform.root.gameObject);
	}

	public virtual bool isEnabled {
		get {
			return this.isActiveAndEnabled;
		}
		set {
			this.enabled = value;
		}
	}

	public abstract bool GetButton (int playerId, InputAction action);
	public abstract bool GetButtonDown (int playerId, InputAction action);
	public abstract bool GetButtonUp (int playerId, InputAction action);
	public abstract float GetAxis (int playerId, InputAction action);
}
