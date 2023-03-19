using UnityEngine;

public class FireBarrel : MonoBehaviour
{
    [SerializeField] [Range(0, 1)]
    private float chanceOfTurningToFireEnemy = 1f;
    [SerializeField] GameObject fireEnemyPrefab;
    [SerializeField] private BoxCollider2D fireCollider;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.IsTouching(fireCollider) && collision.TryGetComponent(out Player player))
        {
            player.OnDie();
            return;
        }

        float spawnChance = Random.Range(0f, 1f);
        if(spawnChance <= chanceOfTurningToFireEnemy)
        {
            Instantiate(fireEnemyPrefab, transform.position, Quaternion.identity);
            Destroy(collision.gameObject);
            chanceOfTurningToFireEnemy = 0;
        }
    }
}
