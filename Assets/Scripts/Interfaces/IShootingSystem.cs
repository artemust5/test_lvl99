using System;

public interface IShootingSystem
{
    bool IsShooting { get; }
    event Action OnStartShooting;
    event Action OnStopShooting;
}
