using System;
using UnityEngine;


public class PlayerMovement : MonoBehaviour
{
    private bool fell;
    private PlayerInput _input;
    private Rigidbody2D _rigidbody;
    [SerializeField] private PlayerState currentPlayerState;
    [SerializeField] private Transform visualTransform;
    [SerializeField] private BoxCollider2D physicalCollider;
    [SerializeField] private BoxCollider2D triggerCollider;
    [SerializeField] private float moveSpeed = 3f;

    [Header("Jumping")]
    [SerializeField] private float jumpHeight = 4.3f; // The height of the jump
    [SerializeField] private float timeToJumpApex = 0.5f; // The time it takes to reach the apex of the jump
    [SerializeField] private float maxFallSpeed = -1f;
    private float jumpVelocity; // The vertical velocity of the jump
    private float gravity; // The gravity force acting on the character

    private const float groundedCheckLength = 0.02f;
    private bool isGrounded;
    private float boundWidth;

    [Header("Climbing")]
    Vector3 lastPlayerPosition;
    private bool canClimb;
    private Transform currentLadder;


    private Vector2 _movementDirection;
    private Collider2D[] _collisionResults = new Collider2D[4];

    private LayerMask groundLayer;
    private LayerMask ladderLayer;

    private void Awake()
    {
        _input = new PlayerInput();
        _input.Initialize();
        _input.OnJumpPressed += Jump;

        _rigidbody = GetComponent<Rigidbody2D>();

        SetPlayerState(PlayerState.Walking);

        groundLayer = LayerMask.GetMask("Ground");
        ladderLayer = LayerMask.NameToLayer("Ladder");
        boundWidth = physicalCollider.bounds.max.x - physicalCollider.bounds.min.x;
    }
    private void Start()
    {
        gravity = -(2 * jumpHeight) / Mathf.Pow(timeToJumpApex, 2);

        // Set the vertical velocity of the jump
        jumpVelocity = Mathf.Abs(gravity) * timeToJumpApex;
        jumpVelocity *= 15.3f; // Increase the jump velocity a bit
    }

    private void Jump()
    {
        if (isGrounded)
        {
            // Uncomment to test jump heights
            gravity = -(2 * jumpHeight) / Mathf.Pow(timeToJumpApex, 2);
            jumpVelocity = Mathf.Abs(gravity) * timeToJumpApex;
            jumpVelocity *= 1.3f;

            // Set the character's y velocity to the jump velocity
            Vector3 velocity = _rigidbody.velocity;
            velocity.y = jumpVelocity;
            _movementDirection = velocity;

            SetPlayerState(PlayerState.Jumping);
        }
    }

    private void Update()
    {
        SetGrounded();
        SetCanClimb();

        StateUpdate();

        MoveCharacter();
    }

    private void StateUpdate()
    {
        switch (currentPlayerState)
        {
            case PlayerState.Climbing:

                if (canClimb)
                    _movementDirection.y = _input.GetMovementVector().y * moveSpeed;
                else
                {
                    transform.position = lastPlayerPosition;
                    _movementDirection.y = 0;
                }
                _movementDirection.x = 0;

                if (isGrounded && _input.GetMovementVector().x != 0)
                    SetPlayerState(PlayerState.Walking);
                break;
            case PlayerState.Jumping:
                ApplyFallingGravity();

                if (isGrounded && _movementDirection.y < 0)
                    SetPlayerState(PlayerState.Walking);

                break;
            case PlayerState.Walking:
                
                if (canClimb && _input.GetMovementVector().y != 0)
                {
                    SetPlayerState(PlayerState.Climbing);
                    break;
                }

                _movementDirection.x = _input.GetMovementVector().x * moveSpeed;
                if (!isGrounded)
                {
                    fell = true;
                    ApplyFallingGravity();
                }
                else
                    _movementDirection.y = gravity * Time.deltaTime;
                
                // Flip sprite correct direction
                if (_movementDirection.x != 0)
                {
                    visualTransform.rotation = _movementDirection.x < 0 ?
                        Quaternion.Euler(Vector3.zero) : Quaternion.Euler(new Vector3(0f, 180f, 0f));
                }

                break;
            case PlayerState.Hammer:
                break;
            default:
                break;
        }
    }

    private void ApplyFallingGravity()
    {
        // Apply gravity to the character's y velocity
        Vector3 velocity = _movementDirection;
        velocity.y += gravity * Time.deltaTime;
        velocity.y = Mathf.Max(velocity.y, maxFallSpeed); // Cap fall speed
        _movementDirection = velocity;
    }

    private void MoveCharacter()
    {
        _rigidbody.velocity = moveSpeed * new Vector3(_movementDirection.x, _movementDirection.y, 0f);
    }

    private void SetPlayerState(PlayerState newState)
    {
        if (currentPlayerState != newState)
            currentPlayerState = newState;

        switch (currentPlayerState)
        {
            case PlayerState.Climbing:
                //Move the player to the center of the ladder
                Vector3 playerPosition = transform.position;
                if (currentLadder != null)
                    playerPosition.x = currentLadder.position.x;
                transform.position = playerPosition;
                break;
            case PlayerState.Jumping:
                break;
            case PlayerState.Walking:
                if (fell)
                {
                    //TODO: Player dies
                }
                break;
            case PlayerState.Hammer:
                break;
            default:
                break;
        }
    }

    private void SetCanClimb()
    {
        Vector2 size = triggerCollider.bounds.size;
        size.y += 0.1f;
        size.x /= 2f;
        
        int amountOfObjectsCurrentlyTouching = Physics2D.OverlapBoxNonAlloc(transform.position, size, 0f, _collisionResults);
        
        canClimb = false;
        bool ladderFound = false;
        for (int i = 0; i < amountOfObjectsCurrentlyTouching; i++)
        {
            GameObject hit = _collisionResults[i].gameObject;
            if (hit.layer == ladderLayer)
            {
                canClimb = true;
                lastPlayerPosition = transform.position;
                ladderFound = true;
                currentLadder = _collisionResults[i].transform;
            }
            else if(_collisionResults[i].transform.position.y > transform.position.y)
            {
                Physics2D.IgnoreCollision(physicalCollider, _collisionResults[i]);
            }
        }

        if (!ladderFound)
            currentLadder = null;
    }

    private void SetGrounded()
    {
        bool grounded = false;
        
        if(Physics2D.BoxCast(physicalCollider.bounds.center + Vector3.down * (physicalCollider.bounds.size.y / 1.9f),
            new Vector2(boundWidth, 0.01f), 0, Vector3.down, groundedCheckLength, groundLayer)){
            grounded = true;
        }

        isGrounded = grounded;
        //if (isGrounded)
        //    SetPlayerState(PlayerState.Walking);
    }

    private void OnDrawGizmosSelected()
    {
        float boundWidth = physicalCollider.bounds.max.x - physicalCollider.bounds.min.x;
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(physicalCollider.bounds.center + Vector3.down * (physicalCollider.bounds.size.y / 1.9f),
            new Vector2(boundWidth, 0.01f));


        Vector2 size = triggerCollider.bounds.size;
        size.y += 0.1f;
        size.x /= 2f;
        Gizmos.DrawWireCube(transform.position, size);
        //Gizmos.DrawLine(physicalCollider.bounds.min, physicalCollider.bounds.min + Vector3.down * groundedCheckLength);
        //Gizmos.color = Color.blue;
        //Vector3 rightSide = new Vector3(physicalCollider.bounds.min.x + boundWidth, physicalCollider.bounds.min.y);
        //Gizmos.DrawLine(rightSide, rightSide + Vector3.down * groundedCheckLength);
    }
}
