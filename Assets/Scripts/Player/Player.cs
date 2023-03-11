using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    private SpriteRenderer spriteRenderer;
    public Sprite[] runSprites;
    public Sprite[] climbSprites;
    //public Sprite climbSprite;
    private int spriteIndex;
    private float vertical;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }
}
