using UnityEngine;

public enum BodyPartType
{
    Head, Torso, Legs, LeftArm, RightArm
}

[RequireComponent(typeof(Collider2D))]
public class BodyPart : MonoBehaviour {

	public HUD HUD;
    public Sprite Sprite;
    public BodyPartType Type;

	void OnTriggerEnter2D(Collider2D other)
	{
		HUD.BodyColliderEnter(true, this);
	}

	void OnTriggerExit2D(Collider2D other)
	{
		HUD.BodyColliderEnter(false, this);
	}

}
