using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnityInputManager : InputManager {

	[SerializeField]
	private string _playerAxisPrefix = "Player";

	[SerializeField]
	private int _maxNumberOfPlayers = 1;

	[Header ("Unity Axis Mapping")]
	[SerializeField]
	private string _fireAxis = "Jump";

	[SerializeField]
	private string _rollAxis = "Dash";

	[SerializeField]
	private string _interactAxis = "Interact";

	[SerializeField]
	private string _moveXAxis = "Horizontal";

	[SerializeField]
	private string _moveYAxis = "Vertical";

	private Dictionary<int, string>[] _actions;

	protected override void Awake () {
		base.Awake ();

		if (InputManager.instance != null) {
			isEnabled = false;
			return;
		}

		SetInstance (this);

		_actions = new Dictionary<int, string>[_maxNumberOfPlayers];

		for (int i = 0; i < _maxNumberOfPlayers; i++) {
			Dictionary<int, string> playerActions = new Dictionary<int, string> ();
			_actions [i] = playerActions;
			string prefix = !string.IsNullOrEmpty (_playerAxisPrefix) ? _playerAxisPrefix + i : string.Empty;
			AddAction (InputAction.Fire, prefix + _fireAxis, playerActions);
			AddAction (InputAction.Roll, prefix + _rollAxis, playerActions);
			AddAction (InputAction.Interact, prefix + _interactAxis, playerActions);
			AddAction (InputAction.MoveX, prefix + _moveXAxis, playerActions);
			AddAction (InputAction.MoveY, prefix + _moveYAxis, playerActions);
			//Debug.Log (prefix);
		}
	}

	private static void AddAction (InputAction action, string actionName, Dictionary<int, string> actions) {
		if (string.IsNullOrEmpty (actionName)) {
			return;
		}

		actions.Add ((int)action, actionName);
	}

	public override bool GetButton (int playerId, InputAction action)
	{
		bool value = Input.GetButton(_actions[playerId][(int)action]);
		return value;
	}

	public override bool GetButtonDown (int playerId, InputAction action)
	{
		bool value = Input.GetButtonDown(_actions[playerId][(int)action]);
		return value;
	}

	public override bool GetButtonUp (int playerId, InputAction action)
	{
		bool value = Input.GetButtonUp(_actions[playerId][(int)action]);
		return value;
	}

	public override float GetAxis (int playerId, InputAction action)
	{
		float value = Input.GetAxisRaw(_actions[playerId][(int)action]);
		return value;
	}


}
