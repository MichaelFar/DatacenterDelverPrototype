using Godot;
using System;

public partial class CameraRotationParent : Node3D
{
	// Called when the node enters the scene tree for the first time.

	[Export]private Node3D nodeToFollow;
	[Export]private Node3D iconParent;
	
	public override void _Ready()
	{
		
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		Rotation = new Vector3(Rotation.X, (float)(Rotation.Y + (delta)), Rotation.Z);
		iconParent.Position = nodeToFollow.Position;
	}
}
