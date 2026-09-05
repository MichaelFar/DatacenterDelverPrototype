using Godot;
using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

public partial class DamageableObject : StaticBody3D, IDamageable
{
	// Called when the node enters the scene tree for the first time.
	[Export] float totalHealth = 100.0f;
	[Export] float currentHealth = 0.0f;
	[Signal]public delegate void ev_HealthDepletedEventHandler();
	public override void _Ready()
	{
		currentHealth = totalHealth;
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}

    public void TakeDamage(float damage_to_take)
    {
        currentHealth -= damage_to_take;
		currentHealth = Mathf.Max(currentHealth, 0.0f);
		if(currentHealth == 0)
		{
			EmitSignal(SignalName.ev_HealthDepleted);
		}
		
    }

    public bool CanTakeDamage()
    {
       	return true;
    }

    public void HealAmount(float health_to_gain)
    {
        currentHealth += health_to_gain;
		currentHealth = Mathf.Min(currentHealth, totalHealth);
		GD.Print("Health is " + currentHealth);
    }

    public void SetHealth(float new_health)
    {
        currentHealth = new_health;
    }

    public void SetMaxHealth(float new_max_health)
    {
        totalHealth = new_max_health;
    }
}
