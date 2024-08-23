using UnityEngine;
using UnityEngine.Tilemaps;

namespace MiniMinerUnity
{
    public class PlayerController : MonoBehaviour
    {
        public Game game;

        public float MovementAmountPerAxis = 1.0f / 16.0f;
        public float MovementUpdates = 1.0f / 20.0f;

        [Header("Scene")]
        [SerializeField] private Tilemap terrain;
        [SerializeField] private TilemapCollider2D terrainCollider;
        [SerializeField] private TileBase terrainWall;
        [SerializeField] private TileBase terrainFloor;

        [Space]
        public Tilemap decorationLayer;
        [SerializeField] private TilemapCollider2D decorationCollider;
        [SerializeField] private TileBase shopKeeperTile;
        [SerializeField] private TileBase blockadeTile;

        [Space]
        [SerializeField] public Animator selector;


        [Header("Graphics")]
        [SerializeField] private Animator animator;
        [SerializeField] private Vector2 movementCollider = new(0.875f, 0.25f);

        private Rigidbody2D rb;
        [SerializeField] private Vector2 facingDirection;

        [Header("Audio")]
        public float FootstepCooldown;
        private float lastFootstepSound;

        private Vector2 Position => new(transform.position.x, transform.position.y);

        public bool EnableInput = true;

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

            var movementDirection = Vector2.zero;

            if (EnableInput)
            {
                movementDirection = GameboyInput.Instance.GameboyControls.Move.ReadValue<Vector2>();
            }

            var movementDelta = movementDirection * MovementAmountPerAxis;

            if (movementDirection.magnitude > 0.1f)
            {
                if ((lastFootstepSound + FootstepCooldown) < Time.realtimeSinceStartup)
                {
                    lastFootstepSound = Time.realtimeSinceStartup;
                    AudioManager.Play(game.Setup.StepSound);
                }

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
                    new(halfCollider.x, halfCollider. y),
                    new(-halfCollider.x, halfCollider. y),
                    new(halfCollider.x, -halfCollider. y),
                    new(-halfCollider.x, -halfCollider. y)
                })
            {
                var samplePoint = Position + corner + movement;
                var sampleTilePosition = Vector3Int.FloorToInt(samplePoint);

                if (terrainCollider.OverlapPoint(samplePoint))
                {
                    willCollide = true;
                }
                if (decorationCollider.OverlapPoint(samplePoint))
                {
                    willCollide = true;
                }
            }

            if (!willCollide)
            {
                transform.position = transform.position + new Vector3(movement.x, movement.y, 0.0f);
                game.State.Player.Position = new Vector2(transform.position.x, transform.position.y);
            }
        }

        private void Update()
        {
            var facingTile = FacingTile;

            if (CanMine || CanTalkToShopKeeper || CanInteractWithBlockade)
            {
                selector.transform.position = new Vector3(facingTile.x + 0.5f, facingTile.y + 0.5f, 0.0f);
                selector.gameObject.SetActive(true);
            }
            else
            {
                selector.gameObject.SetActive(false);
            }
        }

        public Vector3Int FacingTile
        {
            get
            {
                var samplePoint = Position + new Vector2(0.0f, 0.0f) + (facingDirection * 0.85f);
                var sampleTile = Vector3Int.FloorToInt(samplePoint);

                return sampleTile;
            }
        }

        public bool CanMine
        {
            get
            {
                var otherTile = terrain.GetTile(FacingTile);
                return otherTile == terrainWall;
            }
        }

        public bool CanTalkToShopKeeper
        {
            get
            {
                var otherTile = decorationLayer.GetTile(FacingTile);
                return otherTile == shopKeeperTile;
            }
        }

        public bool CanInteractWithBlockade
        {
            get
            {
                var otherTile = decorationLayer.GetTile(FacingTile);
                return otherTile == blockadeTile;
            }
        }
    }
}
