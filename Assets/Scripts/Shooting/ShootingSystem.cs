using UnityEngine;
using System;

public class ShootingSystem : MonoBehaviour, IShootingSystem
{
    [SerializeField] private GameObject projectilePrefab;
    [SerializeField] private float projectileSpeed;
    [SerializeField] private float fireRate = 1f;
    private float nextFireTime = 0f;
    [SerializeField] private float radius = 2f;
    private ITargetFinder targetFinder;
    private GameObject target;
    public bool IsShooting { get; private set; }

    public event Action OnStartShooting;
    public event Action OnStopShooting;

    void Start()
    {
        targetFinder = GetComponent<ITargetFinder>();
        if (targetFinder == null)
        {
            Debug.LogError("TargetFinder component not found!");
        }
    }

    void Update()
    {
        if (targetFinder != null)
        {
            target = targetFinder.FindTarget(transform.position, radius);
            if (target != null && Time.time >= nextFireTime)
            {
                Shoot();
                nextFireTime = Time.time + 1f / fireRate;
                if (!IsShooting)
                {
                    IsShooting = true;
                    OnStartShooting?.Invoke();
                }
            }
            else if (IsShooting && Time.time >= nextFireTime)
            {
                IsShooting = false;
                OnStopShooting?.Invoke();
            }
        }
    }

    private void Shoot()
    {
        if (target != null)
        {
            Vector3 spawnPosition = transform.position + (target.transform.position - transform.position).normalized * 0.5f;
            GameObject projectileObject = Instantiate(projectilePrefab, spawnPosition, Quaternion.identity);
            Vector2 direction = (target.transform.position - transform.position).normalized;
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            angle -= 90f;
            projectileObject.transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle));
            projectileObject.GetComponent<IProjectile>()?.Launch(direction, projectileSpeed, gameObject.tag);
        }
    }
}
