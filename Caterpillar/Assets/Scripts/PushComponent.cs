using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Collider2D))]
public class PushComponent : MonoBehaviour {

	public float ShiftForce = 5.0f;
	private Rigidbody2D rigidBody;

	void Start()
	{
		rigidBody = GetComponent<Rigidbody2D>();
	}

	void OnTriggerEnter2D(Collider2D other)
	{
		rigidBody.AddForce(new Vector2(other.transform.right.x, other.transform.right.y) * ShiftForce, ForceMode2D.Impulse);
	}

}
