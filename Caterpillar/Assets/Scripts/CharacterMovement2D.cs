using UnityEngine;

public class CharacterMovement2D : MonoBehaviour {

	public float Speed = 1f;
	public float RotSpeed = 10f;

	/*public Animator Animator;
	public AudioSource AudioSource;
	private float _lastSpot;
	*/

	void Update()
	{
		float horizontal = Input.GetAxis("Horizontal");
		float vertical = Input.GetAxis("Vertical");

		transform.Translate(new Vector3(horizontal, vertical, 0) * Speed * Time.deltaTime);
		transform.Rotate (new Vector3 (0, 0, -horizontal) * RotSpeed * Time.deltaTime);

		/*
		if (horizontal > 0) {
			Animator.SetBool("isMoving", true);
			Animator.SetBool("facingRight", true);
		}
		else if (horizontal < 0) {
			Animator.SetBool("isMoving", true);
			Animator.SetBool("facingRight", false);
		}
		else if (vertical > 0) {
			Animator.SetBool("isMoving", true);
			Animator.SetBool("facingUp", true);
		}
		else if (vertical < 0) {
			Animator.SetBool("isMoving", true);
			Animator.SetBool("facingUp", false);
		} 
		else {
			Animator.SetBool("isMoving", false);
		}

		if ((Mathf.Abs(vertical) > 0.001 || Mathf.Abs(horizontal) > 0.001) && !AudioSource.isPlaying) {
			AudioSource.time = _lastSpot;
			AudioSource.Play();
		} else if (Mathf.Abs(vertical) < 0.001 && Mathf.Abs(horizontal) < 0.001 && AudioSource.isPlaying) {
			_lastSpot = AudioSource.time;
			AudioSource.Stop();
		}
		*/
	}
}