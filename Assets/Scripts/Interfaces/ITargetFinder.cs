using UnityEngine;

public interface ITargetFinder
{
    GameObject FindTarget(Vector2 position, float radius);
}
