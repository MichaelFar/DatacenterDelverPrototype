using Godot;
using System;

public partial class TestInteractionArea : Area3D, IInteractable
{
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}

    public void OnInteract()
    {
        GD.Print("Interacted with object");
    }

    public bool CanInteract()
    {
        return true;
    }

}
