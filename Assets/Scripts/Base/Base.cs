using UnityEngine;

public class Base : MonoBehaviour
{
    private IHealthSystem healthSystem;
    private IGameManager gameManager;
    [SerializeField] private LayerMask enemyLayer;
    [SerializeField] private LayerMask defenderLayer;
    [SerializeField] private float detectionRadius = 1f;
    [SerializeField] private GameObject[] lifeObjects;
    private int currentLifeIndex;

    void Start()
    {
        healthSystem = GetComponent<IHealthSystem>();
        gameManager = GameManager.Instance;

        if (healthSystem != null)
        {
            healthSystem.OnDied += OnBaseDestroyed;
        }

        currentLifeIndex = lifeObjects.Length - 1;
    }

    void Update()
    {
        CheckCollision();
    }

    private void CheckCollision()
    {
        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(transform.position, detectionRadius, enemyLayer);
        Collider2D[] hitDefenders = Physics2D.OverlapCircleAll(transform.position, detectionRadius, defenderLayer);

        foreach (Collider2D enemy in hitEnemies)
        {
            gameManager.EndGame(false);
        }

        foreach (Collider2D defender in hitDefenders)
        {
            TakeDamage();
            Destroy(defender.gameObject);
        }
    }

    private void TakeDamage()
    {
        if (currentLifeIndex >= 0)
        {
            lifeObjects[currentLifeIndex].SetActive(false);
            currentLifeIndex--;

            if (currentLifeIndex < 0)
            {
                gameManager.EndGame(true);
            }
        }
    }

    private void OnBaseDestroyed()
    {
        gameManager.EndGame(false); 
    }

    void OnDestroy()
    {
        if (healthSystem != null)
        {
            healthSystem.OnDied -= OnBaseDestroyed;
        }
    }
}
