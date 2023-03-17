using UnityEngine;

public class LadderBoundSize : MonoBehaviour
{
    [SerializeField] bool broken;
    [SerializeField] LayerMask groundLayer;
    private float maxCastDistance = 5f;
    BoxCollider2D boxCollider;


    private void Awake()
    {
        
        boxCollider = GetComponent<BoxCollider2D>();
    }

    private void Start()
    {
        Vector2 newSize = boxCollider.size;
        
        if (broken)
        {
            SpriteRenderer spriteRenderer = GetComponent<SpriteRenderer>();
            newSize = new Vector2(0.01f, spriteRenderer.size.y);
            boxCollider.size = newSize;
            return;
        }

        RaycastHit2D topPlatform = Physics2D.Raycast(transform.position, Vector3.up, maxCastDistance, groundLayer);
        RaycastHit2D bottomPlatform = Physics2D.Raycast(transform.position, Vector3.down, maxCastDistance, groundLayer);

        float distanceBetweenPlatforms = Mathf.Abs(topPlatform.point.y - bottomPlatform.point.y);
        float centerPoint = (topPlatform.point.y + bottomPlatform.point.y) / 2;
        float colliderYOffset = centerPoint - transform.position.y + 0.05f;  // Magic number is just to add an extra offset upward so the player can reach it when climbing down from the top

        Vector2 newOffset = boxCollider.offset;
        newOffset.y = colliderYOffset;
        boxCollider.offset = newOffset;

        newSize.y = distanceBetweenPlatforms;
        boxCollider.size = newSize;
    }
}
