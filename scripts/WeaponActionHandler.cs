using Godot;
using System;
using System.Collections.Generic;
using System.ComponentModel;

public partial class WeaponActionHandler : Node3D, IWeaponBehavior
{
	// Called when the node enters the scene tree for the first time.

	[Export] WeaponData weaponData;
	[Export] Node3D firePoint;
	[Signal]public delegate void ev_RecoilPresentEventHandler(Vector2 recoil_vector, float magnitude);
	private bool isReloading = false;

	private float playerHoldDuration = 0.0f;
	private List<Projectile> projectilePool = new List<Projectile>();
	public override void _Ready()
	{
		InstantiateProjectiles();
		weaponData.currentAmmoInMagazine = weaponData.maxAmmoInMagazine;
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}
	private void InstantiateProjectiles()
	{
		for (int i = 0; i < weaponData.maxAmmoInMagazine; i++)
		{
			Projectile projectile_instance = weaponData.projectileScene.Instantiate<Projectile>();
			GetTree().Root.CallDeferred("add_child", projectile_instance);
			projectilePool.Add(projectile_instance);
			projectile_instance.originPoint = firePoint;
		}
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
					playerHoldDuration = 0.0f;
					Shoot();
				}
			}
			else if(Input.IsActionJustReleased("PrimaryFire"))
			{
				playerHoldDuration = 0.0f;
				EmitSignal(SignalName.ev_RecoilPresent, Vector2.Zero, 0.0f);
			}
		}
		else
		{
			if(Input.IsActionJustPressed("PrimaryFire") && CanShoot() && playerHoldDuration == 0.0f)
			{
				playerHoldDuration += delta;
				
				Shoot();
				
				SceneTreeTimer shoot_timer = GetTree().CreateTimer(weaponData.fireCooldown);
				shoot_timer.Timeout += () => {playerHoldDuration = 0.0f;EmitSignal(SignalName.ev_RecoilPresent, Vector2.Zero, 0.0f);};
			}
		}
		
		if(playerHoldDuration == 0.0f && Input.IsActionJustReleased("Reload"))
		{
			Reload();
		}

	}

    public bool CanShoot()
    {
        if(weaponData.currentAmmoInMagazine > 0)
		{
			return true;
		}
		EmitSignal(SignalName.ev_RecoilPresent, Vector2.Zero, 0.0f);
		return false;
    }

    public void Shoot()
    {
		RandomNumberGenerator rand_obj = new RandomNumberGenerator();

		Vector2 rand_direction = new Vector2(rand_obj.RandfRange(0.0f,1.0f),rand_obj.RandfRange(0.0f,1.0f));
        GD.Print("Fired primary");
		int projectile_index = weaponData.maxAmmoInMagazine - weaponData.currentAmmoInMagazine;
		projectilePool[projectile_index].LaunchForward(-firePoint.GlobalBasis.Z);
		EmitSignal(SignalName.ev_RecoilPresent, rand_direction, weaponData.recoil);
		weaponData.currentAmmoInMagazine -=1;
    }

    public void Reload()
    {
		//Need to replace this with a more robust reloading
        weaponData.currentAmmoInMagazine = weaponData.maxAmmoInMagazine;
    }
	
}
