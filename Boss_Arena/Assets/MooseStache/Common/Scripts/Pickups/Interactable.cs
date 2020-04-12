using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interactable : MonoBehaviour {

	public bool Pickable = true;
	public Vector2 PopupOffset;
	public bool spawnsPopup = true;

	protected bool isInside = false;
	protected bool wasInside = false;
	protected Player playerInside = null;

	[Header ("Input")]
	public bool PressToInteract = true;
	public InputAction actionToTrigger;


	protected void Update () {
		if (!wasInside && isInside && spawnsPopup && PressToInteract) {
			if (GameManager.instance != null)
				GameManager.instance.SpawnPopup(new Vector2(transform.position.x, transform.position.y) + PopupOffset);
		}


		if (isInside && playerInside != null && Pickable) {
			if (PressToInteract) {
				if (playerInside.input.GetButtonDown(playerInside.playerNumber, actionToTrigger)) {
					OnPlayerTrigger (playerInside);
				}
			} else {
				OnPlayerTrigger (playerInside);
			}
		}

		wasInside = isInside;
	}

	protected void OnTriggerEnter2DFunc (Collider2D other) {
		if (other.CompareTag ("Entity") && Pickable) {
			var playercomponent = other.GetComponent<Player> ();
			if (playercomponent != null) {
				OnPlayerEnter (playercomponent);
			}
		}
	}

	protected void OnTriggerExit2DFunc (Collider2D other) {
		if (other.CompareTag ("Entity") && Pickable) {
			var playercomponent = other.GetComponent<Player> ();
			if (playercomponent != null) {
				OnPlayerExit (playercomponent);
			}
		}
	}

	protected void OnTriggerStay2DFunc (Collider2D other) {
		if (other.CompareTag ("Entity") && Pickable) {
			var playercomponent = other.GetComponent<Player> ();
			if (playercomponent != null) {
				OnPlayerEnter (playercomponent);
			}
		}
	}


	protected virtual void OnPlayerEnter (Player player) {
		isInside = true;
		playerInside = player;
	}

	protected virtual void OnPlayerExit (Player player) {
		if (isInside && playerInside == player) {
			isInside = false;
			playerInside = null;
			if (GameManager.instance != null && PressToInteract && spawnsPopup)
				GameManager.instance.DespawnPopup ();
		}
	}

	protected virtual void OnPlayerTrigger (Player player) {
		Pickable = false;
		if (GameManager.instance != null && PressToInteract && spawnsPopup)
			GameManager.instance.FadePopup ();
	}

	public virtual void TriggerAction () {
		// Here we drop items, or trigger levers whatever
	}

}
