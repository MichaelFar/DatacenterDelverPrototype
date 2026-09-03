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

	
	public Node3D originPoint;
	public override void _Ready()
	{
		SetEnableProjectile(false);
		lifeTimeTimer.WaitTime = lifeTime;
		lifeTimeTimer.Timeout += () => SetEnableProjectile(false);
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		
	}
	public void LaunchForward(Vector3 launch_direction)
	{
		SetEnableProjectile(true);
		ApplyCentralImpulse(launch_direction * forceMagnitude);
		lifeTimeTimer.Start();
		
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
			LinearVelocity = Vector3.Zero;
			AngularVelocity = Vector3.Zero;
			GlobalPosition = originPoint.GlobalPosition;
			GlobalRotation = originPoint.GlobalRotation;
		}
	}
}
