using Godot;
using System;
using System.ComponentModel;

public partial class WeaponActionHandler : Node3D, IWeaponBehavior
{
	// Called when the node enters the scene tree for the first time.

	[Export] WeaponData weaponData;
	[Export] Node3D firePoint;

	private bool isReloading = false;

	private float playerHoldDuration = 0.0f;
	public override void _Ready()
	{
		
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}

	public void ProcessInput(float delta)
	{
		if(weaponData.fireMode == WeaponData.e_FireType.Automatic)
		{
			if(Input.IsActionPressed("PrimaryFire") && CanShoot())
			{
				playerHoldDuration += delta;
				if(playerHoldDuration >= weaponData.fireCooldown)
				{
					
				}
			}
			else if(Input.IsActionJustReleased("PrimaryFire"))
			{
				playerHoldDuration = 0.0f;
			}
		}
		else
		{
			
		}

		
	}

    public bool CanShoot()
    {
        if(weaponData.currentAmmoInMagazine > 0)
		{
			return true;
		}
		return false;
    }

    public void Shoot()
    {
        throw new NotImplementedException();
    }

    public void Reload()
    {
        throw new NotImplementedException();
    }

    

}
