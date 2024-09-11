using UnityEngine;

public interface IProjectile
{
    void Launch(Vector2 direction, float speed, string shooterTag);
}
