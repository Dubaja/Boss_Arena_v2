using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour {

	public ActionButton PopupPrefab;
	private ActionButton currentlySpawnedPopup;

	public static GameManager instance = null;

	void Awake () {
		if (instance == null) {
			instance = this;
		} else if (instance != this) {
			Destroy (gameObject);
		}

		Application.targetFrameRate = 60;
	}

	void Update () {
		//if (Input.GetKeyDown(KeyCode.R)) {
		//	RestartScene ();
		//}
	}

	public void InvokeRestartScene (float time) {
		Invoke ("RestartScene", time);
	}	

	public void RestartScene () {
		SceneManager.LoadScene (0);
	}

	public void SpawnPopup (Vector2 position) {
		DespawnPopup ();

		currentlySpawnedPopup = Instantiate (PopupPrefab, position, Quaternion.identity) as ActionButton;
	}

	public void DespawnPopup () {
		if (currentlySpawnedPopup != null) {
			currentlySpawnedPopup.DestroySelf();
			currentlySpawnedPopup = null;
		}
	}

	public void FadePopup () {
		if (currentlySpawnedPopup != null) {
			currentlySpawnedPopup.FadeMe ();
			currentlySpawnedPopup = null;
		}
	}
}
