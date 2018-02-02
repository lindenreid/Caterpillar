using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Collider2D))]
public class PushComponent : MonoBehaviour {

	public float ShiftForce = 0.5f;
	private Rigidbody2D rigidBody;

	void Start()
	{
		rigidBody = GetComponent<Rigidbody2D>();
	}

	void OnTriggerEnter2D(Collider2D other)
	{
		Vector2 force = new Vector2 (other.transform.right.x, other.transform.right.y) * ShiftForce * Time.deltaTime;
		rigidBody.AddForce(force, ForceMode2D.Impulse);
	}

}
