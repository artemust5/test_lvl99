using UnityEngine;

public class TargetFinder : MonoBehaviour, ITargetFinder
{
    public GameObject FindTarget(Vector2 position, float radius)
    {
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
        float closestDistance = Mathf.Infinity;
        GameObject closestEnemy = null;

        foreach (GameObject enemy in enemies)
        {
            float distance = Vector2.Distance(position, enemy.transform.position);
            if (distance < closestDistance && distance <= radius)
            {
                closestDistance = distance;
                closestEnemy = enemy;
            }
        }

        return closestEnemy;
    }
}