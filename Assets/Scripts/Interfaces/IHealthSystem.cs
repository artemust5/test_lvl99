using System;

public interface IHealthSystem
{
    int CurrentHealth { get; }
    int MaxHealth { get; }
    void TakeDamage(int amount);
    void Heal(int amount);
    event Action<int> OnHealthChanged;
    event Action OnDied;
}
