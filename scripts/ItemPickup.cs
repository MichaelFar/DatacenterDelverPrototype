using Godot;
using System;
using System.ComponentModel;

public partial class ItemPickup : RigidBody3D
{
	// Called when the node enters the scene tree for the first time.
	[Export] private ItemAnimationData itemAnimationData;
	[Export] private InteractionArea myArea;
	public override void _Ready()
	{
		
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		
	}
	public ItemAnimationData GetItemAnimationData()
	{
		return itemAnimationData;
	}
	public void SetFreeze(bool new_value)
	{
		Freeze = new_value;
	}
	public void SetCanInteract(bool new_value)
	{
		myArea.SetCanInteract(new_value);
	}

    

}
