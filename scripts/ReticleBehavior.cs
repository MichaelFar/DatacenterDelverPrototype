using Godot;
using System;

public partial class ReticleBehavior : TextureRect
{
	// Called when the node enters the scene tree for the first time.
	private Vector2 initialPosition;
	public override void _Ready()
	{
		initialPosition = Position;
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		Position = new Vector2(Position.X, Mathf.Lerp(Position.Y, initialPosition.Y, (float)delta));
	}
}
