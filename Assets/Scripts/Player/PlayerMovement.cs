using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    private PlayerInput _input;
    private Rigidbody2D _rigidbody;
    

    [SerializeField] private Transform visualTransform;
    [SerializeField] private float moveSpeed = 3f;

    [Header("Jumping")]
    [SerializeField] private float jumpHeight = 4.3f; // The height of the jump
    [SerializeField] private float timeToJumpApex = 0.5f; // The time it takes to reach the apex of the jump
    [SerializeField] private float maxFallSpeed = -1f;
    private float jumpVelocity; // The vertical velocity of the jump
    private float gravity; // The gravity force acting on the character
    
    private LayerMask groundLayer;
    float groundedCheckLength = 0.02f;
    private bool isGrounded;
    private float boundWidth;

    private bool isClimbing;

    private Vector2 _movementDirection;
    private Collider2D _collider;
    private Collider2D[] _collisionResults = new Collider2D[4];

    private void Awake()
    {
        _input = new PlayerInput();
        _input.Initialize();
        _input.OnJumpPressed += OnJumpPressed;

        _collider = GetComponent<Collider2D>();
        _rigidbody = GetComponent<Rigidbody2D>();

        groundLayer = LayerMask.GetMask("Ground");
        boundWidth = _collider.bounds.max.x - _collider.bounds.min.x;
    }
    private void Start()
    {
        gravity = -(2 * jumpHeight) / Mathf.Pow(timeToJumpApex, 2);
    }

    private void OnJumpPressed()
    {
        if (isGrounded)
        {
            //Temporary to test jump height
            gravity = -(2 * jumpHeight) / Mathf.Pow(timeToJumpApex, 2);

            // Set the vertical velocity of the jump
            jumpVelocity = Mathf.Abs(gravity) * timeToJumpApex;
            jumpVelocity *= 1.3f; // Increase the jump velocity a bit

            // Set the character's y velocity to the jump velocity
            Vector3 velocity = _rigidbody.velocity;
            velocity.y = jumpVelocity;
            _rigidbody.velocity = velocity;

            isGrounded = false;
        }
    }

    private void Update()
    {


        SetGroundedState();
        CheckCollision();

        //if (isClimbing)
        //{
        //    // direction.y = Input.GetAxis("Vertical") * _moveSpeed;
        //    _movementDirection.y = _input.GetMovementVectorNormalized().y * moveSpeed;
        //}
 
        _movementDirection.x = _input.GetMovementVectorNormalized().x * moveSpeed;

        if (!isGrounded)
        {
            // Apply gravity to the character's y velocity
            Vector3 velocity = _rigidbody.velocity;
            velocity.y += gravity * Time.deltaTime;
            velocity.y = Mathf.Max(velocity.y, maxFallSpeed); // Cap fall speed
            _rigidbody.velocity = velocity;
        }
        else if(isGrounded && _rigidbody.velocity.y <= 0)
        {
            Vector3 velocity = _rigidbody.velocity;
            velocity.y = 0;
            _rigidbody.velocity = velocity;

            
        }

        // Flip sprite correct direction
        if (_movementDirection.x != 0)
        {
            visualTransform.rotation = _movementDirection.x < 0 ? 
                Quaternion.Euler(Vector3.zero) : Quaternion.Euler(new Vector3(0f, 180f, 0f));

            SetYPosition();
        }

        MoveCharacter();
        
    }

    private void SetYPosition()
    {
        RaycastHit2D hit = Physics2D.BoxCast(_collider.bounds.center + Vector3.down * (_collider.bounds.size.y / 1.9f),
                            new Vector2(boundWidth, 0.01f), 0, Vector3.down, groundedCheckLength, groundLayer);
        if (hit.collider is null) return;

        float offset = 0.01f;
        Vector3 playerPosition = transform.position;
        playerPosition.y = hit.point.y + offset;
        transform.position = playerPosition;     
    }

    private void MoveCharacter()
    {
        transform.position += moveSpeed * Time.deltaTime * new Vector3(_movementDirection.x, 0f, 0f);
    }

    private void CheckCollision()
    {
        
        Vector2 size = _collider.bounds.size;
        size.y += 0.1f;
        size.x /= 2f;

        
        int amount = Physics2D.OverlapBoxNonAlloc(transform.position, size, 0f, _collisionResults);

        for (int i = 0; i < amount; i++)
        {
            GameObject hit = _collisionResults[i].gameObject;

            
            if (hit.layer == LayerMask.NameToLayer("Ladder"))
            {
                isClimbing = true;
            }
        }
    }

    private void SetGroundedState()
    {
        bool grounded = false;
        Vector3 boundsMin = _collider.bounds.min;
        Vector3 boundsMax = _collider.bounds.min + (Vector3.right * boundWidth);
        
        //if (Physics2D.Raycast(boundsMin, Vector2.down, groundedCheckLength, groundLayer) ||
        //    Physics2D.Raycast(boundsMax, Vector2.down, groundedCheckLength, groundLayer))
        
        if(Physics2D.BoxCast(_collider.bounds.center + Vector3.down * (_collider.bounds.size.y / 1.9f),
            new Vector2(boundWidth, 0.01f), 0, Vector3.down, groundedCheckLength, groundLayer)){
            grounded = true;
        }

        isGrounded = grounded;
    }

    private void OnDrawGizmosSelected()
    {
        Collider2D _collider = GetComponent<Collider2D>();
        float boundWidth = _collider.bounds.max.x - _collider.bounds.min.x;
        Gizmos.color = Color.red;
        Gizmos.DrawLine(_collider.bounds.min, _collider.bounds.min + Vector3.down * groundedCheckLength);
        Gizmos.color = Color.blue;
        Vector3 rightSide = new Vector3(_collider.bounds.min.x + boundWidth, _collider.bounds.min.y);
        Gizmos.DrawLine(rightSide, rightSide + Vector3.down * groundedCheckLength);
    }
}
