using UnityEngine;

public class Projectile : MonoBehaviour, IProjectile
{
    public float damage;
    private Vector2 direction;
    private float speed;
    [SerializeField] private string shooterTag;

    public void Launch(Vector2 direction, float speed, string shooterTag)
    {
        this.direction = direction;
        this.speed = speed;
        this.shooterTag = shooterTag; 
    }

    void Update()
    {
        transform.position += (Vector3)direction * speed * Time.deltaTime;
        CheckCollision();
    }

    private void CheckCollision()
    {
        RaycastHit2D hit = Physics2D.Raycast(transform.position, direction, speed * Time.deltaTime);
        if (hit.collider != null && hit.collider.gameObject != gameObject)
        {
            IHealthSystem healthSystem = hit.collider.GetComponent<IHealthSystem>();
            if (healthSystem != null && hit.collider.tag != shooterTag)
            {
                healthSystem.TakeDamage((int)damage);
                Destroy(gameObject);
            }
        }
    }
}
