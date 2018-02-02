using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ReactionText : MonoBehaviour {

	public Text text;
	public float lifetime = 5.0f;
	private float timeLeft;

	public void Show(string newText) {
		text.text = newText;
		gameObject.SetActive (true);
		timeLeft = lifetime;
	}
	
	void Update () {
		timeLeft -= Time.deltaTime;	
		if (timeLeft <= 0) {
			gameObject.SetActive (false);
		}
	}
}
