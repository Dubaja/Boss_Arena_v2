using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Door : Interactable {

	[Header ("Door")]
	public bool Opened = false;
	[Header ("Animator")]
	public Animator animator;

	new void Update () {
		base.Update ();

		// Check if all enemies have died 2 times per second (every 0.5f secs)
		InvokeRepeating ("CheckForEnemiesToOpen", 0.5f, 0.5f);
	}

	// Method to invoke
	public void CheckForEnemiesToOpen () {
		if (!Opened) {
			if (!AreEnemiesAlive()) {
				Pickable = true;
				Opened = true;

				if (CameraShaker.instance != null) {
					CameraShaker.instance.InitShake(0.2f, 1f);
				}

				if (!animator.GetCurrentAnimatorStateInfo (0).IsName ("Open")) {
					animator.Play ("Open");
				}
			}
		}
	}

	// Method which actually checks if there are objects with the Enemies tag "alive" 
	public bool AreEnemiesAlive () {
		var enemies = GameObject.FindGameObjectWithTag ("Enemies");

		if (enemies != null) {
			return true;
		} else {
			return false;
		}
	}

	void OnTriggerEnter2D(Collider2D col) {
		OnTriggerEnter2DFunc (col);
	}

	void OnTriggerStay2D(Collider2D col) {
		OnTriggerStay2DFunc (col);
	}

	void OnTriggerExit2D(Collider2D col) {
		OnTriggerExit2DFunc (col);
	}

	protected override void OnPlayerTrigger (Player player)
	{
		base.OnPlayerTrigger (player);
	
		//SceneManager.LoadScene (0);
		GameManager.instance.RestartScene();
	}
}
