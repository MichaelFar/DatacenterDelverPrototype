using Godot;
using System;
using System.Collections.Generic;
using System.ComponentModel;

public partial class WeaponActionHandler : Node3D, IWeaponBehavior
{
	// Called when the node enters the scene tree for the first time.

	[Export] WeaponData weaponData;
	[Export] Node3D firePoint;
	[Export] Timer shootTimer;
	[Signal]public delegate void ev_RecoilPresentEventHandler(Vector2 recoil_vector, float magnitude);
	private bool isReloading = false;

	private float playerHoldDuration = 0.0f;
	private List<Projectile> projectilePool = new List<Projectile>();
	private Vector2 currentRecoilDirection;
	private bool hasFired = false;


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
	private float firedTimer = 0.0f;
	float current_delta_coefficient = 1.0f;
	public void ProcessInput(float delta)
	{
		float currentCooldown = weaponData.fireCooldown;
		
		if(hasFired)
		{
			
			firedTimer += delta * current_delta_coefficient;
			firedTimer = Mathf.Clamp(firedTimer, 0.0f, 1.0f);
			if(weaponData.recoilCurve == null)
			{
				EmitSignal(SignalName.ev_RecoilPresent, currentRecoilDirection, weaponData.recoil * Mathf.Abs(1.0f - (firedTimer / weaponData.fireCooldown)));
			}
			else
			{
				EmitSignal(SignalName.ev_RecoilPresent, currentRecoilDirection, weaponData.recoil * weaponData.recoilCurve.Curve.Sample(Mathf.Abs(1.0f - (firedTimer / weaponData.fireCooldown))));
			}
			
			if(firedTimer >= weaponData.fireCooldown)
			{
				firedTimer = 0.0f;
				hasFired = false;
				//playerHoldDuration = 0.0f;
				
			}
			//GD.Print("Recoil vector is " + )
			
		}
		else
		{
			currentRecoilDirection = Vector2.Zero;
		}
		if(weaponData.fireMode == WeaponData.e_FireType.Automatic)
		{
			if(Input.IsActionPressed("PrimaryFire") && CanShoot())
			{
				if(playerHoldDuration == 0.0f)
				{
					Shoot();
					SceneTreeTimer shoot_timer = GetTree().CreateTimer(Mathf.Clamp(currentCooldown - playerHoldDuration, 0.0f, currentCooldown));
				//firedTimer = weaponData.fireCooldown - firedTimer;
					current_delta_coefficient = 0.8f;
					shoot_timer.Timeout += () => {if(!hasFired){EmitSignal(SignalName.ev_RecoilPresent, Vector2.Zero, 0.0f);playerHoldDuration = 0.0f;}};
				}
				playerHoldDuration += delta;
				if(playerHoldDuration >= weaponData.fireCooldown)
				{
					playerHoldDuration = 0.0f;
					//hasFired = false;
					
				}
				//EmitSignal(SignalName.ev_RecoilPresent, new Vector2().Lerp(Vector2.Zero, playerHoldDuration / weaponData.fireCooldown), weaponData.recoil);
				
			}
			else if(Input.IsActionJustReleased("PrimaryFire"))
			{
				
				
				currentCooldown *= 0.3f;
				firedTimer = 0.0f;
				
			}
		}
		else
		{
			if(Input.IsActionJustPressed("PrimaryFire") && CanShoot() && !hasFired)
			{
				
				
				Shoot();
				current_delta_coefficient = 4.0f;
				SceneTreeTimer shoot_timer = GetTree().CreateTimer(weaponData.fireCooldown);
				shoot_timer.Timeout += () => {if(!hasFired){playerHoldDuration = 0.0f;EmitSignal(SignalName.ev_RecoilPresent, Vector2.Zero, 0.0f);}};// hasFired = false;};
			}
		}
		//EmitSignal(SignalName.ev_RecoilPresent, currentRecoilDirection * (1.0f - (playerHoldDuration / weaponData.fireCooldown)), weaponData.recoil);
		if(!hasFired && Input.IsActionJustReleased("Reload"))
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
		//EmitSignal(SignalName.ev_RecoilPresent, Vector2.Zero, 0.0f);
		return false;
    }

    public void Shoot()
    {
		RandomNumberGenerator rand_obj = new RandomNumberGenerator();
		hasFired = true;
		Vector2 rand_direction = new Vector2(rand_obj.RandfRange(0.3f,1.0f),rand_obj.RandfRange(0.2f,1.0f));
        //GD.Print("Fired primary");
		int projectile_index = weaponData.maxAmmoInMagazine - weaponData.currentAmmoInMagazine;
		projectilePool[projectile_index].spread = weaponData.weaponSpread;
		projectilePool[projectile_index].LaunchForward(-firePoint.GlobalBasis.Z);
		currentRecoilDirection = rand_direction;
		//EmitSignal(SignalName.ev_RecoilPresent, rand_direction, weaponData.recoil);
		weaponData.currentAmmoInMagazine -=1;
    }

    public void Reload()
    {
		//Need to replace this with a more robust reloading
        weaponData.currentAmmoInMagazine = weaponData.maxAmmoInMagazine;
    }
	
}
