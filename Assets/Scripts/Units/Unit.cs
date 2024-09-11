using UnityEngine;

public class Unit : MonoBehaviour
{
    [SerializeField] private float speed;
    private int currentPointIndex = 0;
    public Waypoint waypoint;
    private IHealthSystem healthSystem;
    private IShootingSystem shootingSystem;
    private bool isStopped = false;
    private Vector2 lastPosition;

    void Start()
    {
        healthSystem = GetComponent<IHealthSystem>();
        shootingSystem = GetComponent<IShootingSystem>();

        if (shootingSystem != null)
        {
            shootingSystem.OnStartShooting += StopMoving;
            shootingSystem.OnStopShooting += ResumeMoving;
        }
    }

    void Update()
    {
        if (!isStopped)
        {
            MoveAlongPath();
        }
    }

    private void MoveAlongPath()
    {
        if (waypoint != null && waypoint.points.Length > 0)
        {
            Transform targetTransform = waypoint.GetNextPoint(currentPointIndex);
            if (targetTransform != null)
            {
                Vector2 targetPosition = targetTransform.position;
                Vector2 direction = (targetPosition - (Vector2)transform.position).normalized;
                transform.position += (Vector3)direction * speed * Time.deltaTime;

                if (Vector2.Distance(transform.position, targetPosition) < 0.1f)
                {
                    currentPointIndex++;
                    if (currentPointIndex >= waypoint.points.Length)
                    {
                        currentPointIndex = 0;
                    }
                }
            }
        }
    }

    private void StopMoving()
    {
        isStopped = true;
        lastPosition = transform.position;
    }

    private void ResumeMoving()
    {
        isStopped = false;
        MoveAlongPath();
    }

    void OnDestroy()
    {
        if (shootingSystem != null)
        {
            shootingSystem.OnStartShooting -= StopMoving;
            shootingSystem.OnStopShooting -= ResumeMoving;
        }
    }
}
