using Godot;
using System;
using System.Collections.Generic;


public partial class InteractionHandler : Node3D
{
	// Called when the node enters the scene tree for the first time.

	[Export]ShapeCast3D interactionShape;
	public override void _Ready()
	{
		
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		if(Input.IsActionJustReleased("Interact"))
		{
			ProcessInteractionCollisions();
		}
	}

	public void ProcessInteractionCollisions()
	{
		int num_collisions = interactionShape.GetCollisionCount();
		for (int i = 0; i < num_collisions; i++)
		{
			IInteractable interactable = interactionShape.GetCollider(i) as IInteractable;
			if(interactable != null)
			{
				if(interactable.CanInteract())
				{
					interactable.OnInteract();
				}
			}
		}
	}
	

	

    
}
