using Godot;
using System;

public interface IDamageable
{
    public void TakeDamage(float damage_to_take);
    public bool CanTakeDamage();

    public void HealAmount(float health_to_gain);
    public void SetHealth(float new_health);

    public void SetMaxHealth(float new_max_health);
}
