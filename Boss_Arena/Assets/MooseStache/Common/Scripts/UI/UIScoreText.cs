using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIScoreText : MonoBehaviour {

	public Text text;

	void OnEnable()
	{
		ScoreManager.ScoreUpdated += UpdateScoreUI;
	}
		
	void OnDisable()
	{
		ScoreManager.ScoreUpdated -= UpdateScoreUI;
	}

	void Start () {
		UpdateScoreUI (0);
	}

	public void UpdateScoreUI (int value) {
		if (text != null) {
			text.text = value.ToString ();
		}
	}

}
