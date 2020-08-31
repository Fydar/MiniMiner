using UnityEngine;
using UnityEngine.Tilemaps;

public class PlayerController : MonoBehaviour
{
	public float MovementAmountPerAxis = 1.0f / 16.0f;
	public float MovementUpdates = 1.0f / 20.0f;

	[Header("Scene")]
	[SerializeField] private Tilemap terrain;
	[SerializeField] private TilemapCollider2D terrainCollider;
	[SerializeField] private TileBase terrainWall;
	[SerializeField] private TileBase terrainFloor;

	[Header("Graphics")]
	[SerializeField] private Animator animator;
	[SerializeField] private Vector2 movementCollider = new Vector2(0.875f, 0.25f);

	private Rigidbody2D rb;
	[SerializeField] private Vector2 facingDirection;

	private Vector2 Position => new Vector2(transform.position.x, transform.position.y);

	private void Awake()
	{
		rb = GetComponent<Rigidbody2D>();

		InvokeRepeating("MovementUpdate", MovementUpdates, MovementUpdates);
	}

	private void OnEnable()
	{
		animator.SetFloat("Horizontal", facingDirection.x);
		animator.SetFloat("Vertical", facingDirection.y);
	}

	private void MovementUpdate()
	{
		if (!isActiveAndEnabled)
		{
			return;
		}

		var movementDirection = new Vector2(
			Input.GetAxisRaw("Horizontal"),
			Input.GetAxisRaw("Vertical")
		);
		var movementDelta = movementDirection * MovementAmountPerAxis;

		if (movementDirection.magnitude > 0.1f)
		{
			if (Mathf.Abs(movementDirection.x) > 0.1f)
			{
				facingDirection = new Vector2(movementDirection.x, 0.0f);
			}
			else
			{
				facingDirection = new Vector2(0.0f, movementDirection.y);
			}

			animator.SetFloat("Horizontal", movementDirection.x);
			animator.SetFloat("Vertical", movementDirection.y);
			animator.SetFloat("Speed", 1.0f);
		}
		else
		{
			animator.SetFloat("Speed", 0.0f);
		}

		TakeStep(new Vector2(movementDelta.x, 0.0f));
		TakeStep(new Vector2(0.0f, movementDelta.y));
	}

	private void TakeStep(Vector2 movement)
	{
		var halfCollider = movementCollider * 0.5f;

		bool willCollide = false;
		foreach (var corner in new Vector2[]
			{
				new Vector2(halfCollider.x, halfCollider. y),
				new Vector2(-halfCollider.x, halfCollider. y),
				new Vector2(halfCollider.x, -halfCollider. y),
				new Vector2(-halfCollider.x, -halfCollider. y)
			})
		{
			var samplePoint = Position + corner + movement;
			var sampleTilePosition = Vector3Int.FloorToInt(samplePoint);

			if (terrainCollider.OverlapPoint(samplePoint))
			{
				willCollide = true;
			}
		}

		if (!willCollide)
		{
			transform.position = transform.position + new Vector3(movement.x, movement.y, 0.0f);
		}
	}

	private void Update()
	{
		if (Input.GetKeyDown(KeyCode.C))
		{
			var samplePoint = Position + (facingDirection * 0.65f);
			var sampleTile = Vector3Int.FloorToInt(samplePoint);

			var otherTile = terrain.GetTile(sampleTile);

			// Debug.DrawLine(samplePoint, new Vector3(samplePoint.x, samplePoint.y, 0.0f) + Vector3.forward, Color.red, 1.0f);
		}
	}

	public Vector3Int FacingTile
	{
		get
		{
			var samplePoint = Position + (facingDirection * 0.65f);
			var sampleTile = Vector3Int.FloorToInt(samplePoint);

			return sampleTile;
		}
	}
}
