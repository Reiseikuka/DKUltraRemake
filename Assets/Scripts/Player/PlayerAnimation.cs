using UnityEngine;

public class PlayerAnimation : MonoBehaviour
{
    [SerializeField] private Sprite[] runSprites;
    [SerializeField] private Sprite[] climbSprites;
    [SerializeField] private Sprite[] hammerSprites;
    [SerializeField] private Sprite jumpSprite;

    private SpriteRenderer spriteRenderer;
    private PlayerMovement movement;
    private int spriteIndex = 0;
    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        movement = GetComponentInParent<PlayerMovement>();
    }

    private void OnEnable()
    {
        InvokeRepeating(nameof(AnimateSprite), 1f/12f, 1f/12f);
    }

    private void OnDisable()
    {
        CancelInvoke();
    }

    private void AnimateSprite()
    {
        switch(movement.CurrentPlayerState)
        {
            case PlayerState.Climbing:
                if (movement.Input.GetMovementVector().y != 0)
                {
                    spriteIndex++;
                    spriteIndex %= climbSprites.Length;
                }
                spriteRenderer.sprite = climbSprites[spriteIndex];
                break;
            case PlayerState.Jumping:
                spriteRenderer.sprite = jumpSprite;
                break;
            case PlayerState.Walking:
                if (movement.Input.GetMovementVector().x != 0f)
                {
                    spriteIndex++;
                    spriteIndex %= runSprites.Length;

                }
                spriteRenderer.sprite = runSprites[spriteIndex];
                break;
            case PlayerState.Hammer:
                spriteIndex++;
                spriteIndex %= hammerSprites.Length;

                spriteRenderer.sprite = hammerSprites[spriteIndex];
                break;
            default:
                break;
        };
    }
}
