using UnityEngine;
using System.Collections;

public class HUD : MonoBehaviour {

	public GameObject EatImage;

	public void ShowEatImage(bool show)
	{
		EatImage.SetActive(show);
	}

}
