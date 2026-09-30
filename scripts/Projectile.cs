using Godot;
using System;
using System.ComponentModel.DataAnnotations.Schema;


public partial class Projectile : RigidBody3D
{
	// Called when the node enters the scene tree for the first time.


	public bool isLive = false;
	[Export]public float forceMagnitude = 50.0f;
	[Export]public Timer lifeTimeTimer;

	[Export] public float lifeTime = 10.0f;
	private float currentLifeTime = 0.0f;
	[Export] private MeshInstance3D projectileMesh;
	[Export] private CollisionShape3D collider;

	[Export] private float damage = 10.0f;

	[Export] public int numBounces = 0;
	private int currentBounces = 0;
	RandomNumberGenerator rand_obj = new RandomNumberGenerator();
	public float spread = 0.0f;

	
	public Node3D originPoint;
	public override void _Ready()
	{
		currentBounces = numBounces;
		SetEnableProjectile(false);
		lifeTimeTimer.WaitTime = lifeTime;
		lifeTimeTimer.Timeout += () => SetEnableProjectile(false);
		
		
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		
	}
    public override void _PhysicsProcess(double delta)
    {
        base._PhysicsProcess(delta);
		//LinearVelocity = LinearVelocity.Normalized() * forceMagnitude;
    }

	public void LaunchForward(Vector3 launch_direction)
	{
		
		
		SetEnableProjectile(true);

		launch_direction += -GetSpreadDir(Basis);
		launch_direction = launch_direction.Normalized();
		ApplyCentralImpulse(launch_direction * forceMagnitude);
		lifeTimeTimer.Start();
		
	}
	private Vector3 GetSpreadDir(Basis reference_basis)
	{
		float twist = rand_obj.RandfRange(0, Mathf.Tau);
		Vector3 axis = new Vector3(Mathf.Cos(twist), Mathf.Sin(twist), 0);
		
		float angle = (float)(1.0 -Mathf.Sqrt(1.0 - Mathf.Sqrt(rand_obj.Randf()))) * spread;// # Superior distribution
		//#var angle := sqrt(randf()) * spread_degrees # Uniform spread, use instead if prefered
		
		//# Negative because cameras in Godot point backwards
		return reference_basis.Z.Rotated(reference_basis * axis, angle);
	}
		
	private void SetEnableProjectile(bool new_value)
	{
		
		isLive = new_value;
		projectileMesh.Visible = new_value;
		collider.Disabled = !new_value;
		Freeze = !new_value;
		if(lifeTimeTimer != null)
		{
			lifeTimeTimer.Stop();
		}
		if(new_value)
		{
			currentBounces = numBounces;
			LinearVelocity = Vector3.Zero;
			AngularVelocity = Vector3.Zero;
			GlobalPosition = originPoint.GlobalPosition;
			GlobalRotation = originPoint.GlobalRotation;
		}
	}

	private void DamageOther(Node object_to_damage)
	{
		IDamageable damageable_object = object_to_damage as IDamageable;
		if(damageable_object != null)
		{
			if(damageable_object.CanTakeDamage())
			{
				damageable_object.TakeDamage(damage);
			}
		}
		if(!(currentBounces > 0))
		{
			SceneTreeTimer despawn_timer = GetTree().CreateTimer(0.1);
			despawn_timer.Timeout += () => SetEnableProjectile(false);
		}
		else
		{
			currentBounces -=1;
		}
		
		
	}
}
