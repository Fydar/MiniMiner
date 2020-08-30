using UnityEngine;

public class PlayerController : MonoBehaviour
{
	public float MovementAmountPerAxis = 1.0f / 16.0f;
	public float MovementUpdates = 1.0f / 20.0f;

	[Header("Graphics")]
	[SerializeField] private Animator animator;

	private Rigidbody2D rigidbody;

	private void Awake()
	{
		rigidbody = GetComponent<Rigidbody2D>();

		InvokeRepeating("MovementUpdate", MovementUpdates, MovementUpdates);
	}

	private void MovementUpdate()
	{
		var desiredMovement = new Vector2(
			Input.GetAxisRaw("Horizontal"),
			Input.GetAxisRaw("Vertical")
		);

		if (desiredMovement.magnitude > 0.1f)
		{
			animator.SetFloat("Horizontal", desiredMovement.x);
			animator.SetFloat("Vertical", desiredMovement.y);
			animator.SetFloat("Speed", 1.0f);
		}
		else
		{
			animator.SetFloat("Speed", 0.0f);
		}

		desiredMovement *= MovementAmountPerAxis;
		rigidbody.MovePosition(rigidbody.position + (desiredMovement));
	}
}
