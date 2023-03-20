using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Barrel : MonoBehaviour
{
    [SerializeField] private float speed = 3f;

    private Rigidbody2D myRigidbody;
    private LayerMask groundLayer;

    [Header("Falling")]
    [SerializeField][Range(0,1)] float fallOdds = 1;
    [SerializeField] private Sprite[] fallingSprites;
    [SerializeField] private Sprite defaultSprite;
    [SerializeField] private float ladderCheckDistance = 0.5f;
    private Transform currentLadder;
    private LayerMask ladderLayer;
    private float spriteTransitionTime = 1 / 12;
    private int spriteIndex;
    private SpriteRenderer spriteRenderer;
    private Coroutine fallSpriteAnimation;
    private bool isCurrentlyFalling;
    private Vector2 castBoxSize;

    // TODO: Implement barrel traversal for ladders

    private void Awake()
    {
        myRigidbody = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        groundLayer = LayerMask.NameToLayer("Ground");
        ladderLayer = LayerMask.GetMask("Ladder");
        castBoxSize = transform.localScale;
        castBoxSize.x = 0.1f;
    }

    private void Update()
    {
        if (isCurrentlyFalling) return;
        Physics2D.queriesHitTriggers = true;
        RaycastHit2D ladder = Physics2D.BoxCast(transform.position, castBoxSize, 0, Vector2.down, ladderCheckDistance, ladderLayer);
        if (!ladder) return;
        if (ladder.transform.position.y >= transform.position.y) return;
        if (ladder.transform == currentLadder) return;
        

        currentLadder = ladder.transform;

        float fallChance = Random.Range(0f, 1f);
        if (fallChance > fallOdds) return;

        myRigidbody.angularVelocity = 0;

        Vector3 currentPosition = transform.position;
        currentPosition.x = currentLadder.position.x;
        transform.position = currentPosition;
        
        fallSpriteAnimation = StartCoroutine(Fall());
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.layer == groundLayer)
        {
            isCurrentlyFalling = false;
            myRigidbody.constraints = RigidbodyConstraints2D.None;
            myRigidbody.angularVelocity = 0;
            myRigidbody.velocity = collision.transform.right * speed;

            if (fallSpriteAnimation != null)
                StopCoroutine(fallSpriteAnimation);
            spriteRenderer.sprite = defaultSprite;
        }
    }


    IEnumerator Fall()
    {
        isCurrentlyFalling = true;
        transform.rotation = Quaternion.Euler(Vector3.zero);
        myRigidbody.constraints = RigidbodyConstraints2D.FreezeAll;
        do
        {
            transform.position += Vector3.down * Time.deltaTime * speed;

            //TODO: FOR ARTIST: Need more sprites to simlulate rotation
            spriteRenderer.sprite = fallingSprites[spriteIndex];
            spriteIndex++;
            spriteIndex %= fallingSprites.Length;
            yield return new WaitForSeconds(spriteTransitionTime);
        } while (true);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        castBoxSize = transform.localScale;
        castBoxSize.x = 0.1f;
        castBoxSize.y += ladderCheckDistance;
        Gizmos.DrawWireCube(transform.position + (Vector3.down * ladderCheckDistance), castBoxSize);
    }
}
