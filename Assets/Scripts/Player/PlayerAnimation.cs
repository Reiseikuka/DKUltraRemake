using UnityEngine;

public class PlayerAnimation : MonoBehaviour
{
    PlayerInput input;

    [SerializeField] private Sprite[] runSprites;
    [SerializeField] private Sprite[] climbSprites;
    private void Awake()
    {
        
    }

    private void OnEnable()
    {
        
        // InvokeRepeating(nameof(AnimateSprite), 1f/12f, 1f/12f);
    }


    private void OnDisable()
    {
        CancelInvoke();
    }

    //private void AnimateSprite()
    //{
    //    if (climbing)
    //    {
    //        if (Input.GetAxisRaw("Vertical") != 0)
    //        {
    //            //spriteRenderer.sprite = climbSprite;

    //            spriteIndex++;

    //            if (spriteIndex >= climbSprites.Length)
    //            {
    //                spriteIndex = 0;
    //            }

    //            spriteRenderer.sprite = climbSprites[spriteIndex];
    //        }
    //    }
    //    else if (direction.x != 0f)
    //    {
    //        spriteIndex++;

    //        if (spriteIndex >= runSprites.Length)
    //        {
    //            spriteIndex = 0;
    //        }

    //        spriteRenderer.sprite = runSprites[spriteIndex];
    //    }
    //}
}
