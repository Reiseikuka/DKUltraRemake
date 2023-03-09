using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    private SpriteRenderer spriteRenderer;
    public Sprite[] runSprites;
    //public Sprite climbSprite;
    public Sprite[] climbSprites;
    private int spriteIndex;

    private new Rigidbody2D rigidbody;
    private new Collider2D collider;

    private Collider2D[] results;
    private Vector2 direction;

    private float moveSpeed = 2f;
    private float jumpStrength = 4.3f;

    private bool grounded;
    private bool climbing;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        rigidbody  = GetComponent<Rigidbody2D>();
        collider  = GetComponent<Collider2D>();
        results = new Collider2D[4];
    }

    private void OnEnable()
    {
        InvokeRepeating(nameof(AnimateSprite), 1f/12f, 1f/12f);
    }

    private void OnDisable()
    {
        CancelInvoke();
    }

    private void CheckCollision()
    {
        grounded = false;
        climbing = false;

        Vector2 size = collider.bounds.size;
        size.y += 0.1f;
        size.x /= 2f;

        int amount = Physics2D.OverlapBoxNonAlloc(transform.position, size, 0f,results);

        for (int i = 0; i < amount; i++)
        {
            GameObject hit = results[i].gameObject;

            if (hit.layer == LayerMask.NameToLayer("Ground"))
            {
                grounded = hit.transform.position.y < (transform.position.y - 0.5f);

                Physics2D.IgnoreCollision(collider, results[i], !grounded);
            }
            else if (hit.layer == LayerMask.NameToLayer("Ladder"))
            {
                climbing = true;
            }
        }
    }

    private void Update()
    {

        CheckCollision();

        if (climbing)
        {
            direction.y = Input.GetAxis("Vertical")  * moveSpeed;
        } else if (grounded && Input.GetButtonDown("Jump"))
        {
            direction = Vector2.up * jumpStrength;
        } else
        {
            direction += Physics2D.gravity * Time.deltaTime;
        }

        direction.x = Input.GetAxis("Horizontal") * moveSpeed;

        if (grounded)
        {
           direction.y = Mathf.Max(direction.y, -1f);
        }

        //<>
        if (direction.x < 0f)
        {
            transform.eulerAngles = Vector3.zero;
        } else if (direction.x > 0f)
        {
            transform.eulerAngles = new Vector3(0f, 180f,0f);
        }

    }

    private void FixedUpdate()
    {
        rigidbody.MovePosition(rigidbody.position + direction * Time.fixedDeltaTime);
    }

    private void AnimateSprite()
    {
        if (climbing)
        {
            //spriteRenderer.sprite = climbSprite;

            spriteIndex++;

            if (spriteIndex >= climbSprites.Length)
            {
                spriteIndex = 0;
            }

            spriteRenderer.sprite = climbSprites[spriteIndex];
        }
        else if (direction.x != 0f)
        {
            spriteIndex++;

            if (spriteIndex >= runSprites.Length)
            {
                spriteIndex = 0;
            }

            spriteRenderer.sprite = runSprites[spriteIndex];
        }
    }
}
