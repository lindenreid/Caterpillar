using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Collider2D))]
public class BodyPart : MonoBehaviour {

	public HUD HUD;

	void OnTriggerEnter2D(Collider2D other)
	{
		HUD.ShowEatImage(true);
	}

	void OnTriggerExit2D(Collider2D other)
	{
		HUD.ShowEatImage(false);
	}

}
