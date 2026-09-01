using Godot;
using System;
using System.Collections.Generic;
using System.ComponentModel;


public partial class InteractionHandler : Node3D
{
	// Called when the node enters the scene tree for the first time.

	[Export]ShapeCast3D interactionShape;
	[Export]InteractPrompt interactHudElement;
	
	[Signal]public delegate void ev_ThisColliderInteractedWithEventHandler(Node3D delivered_collider);

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
		ShowOrHideInteractPrompt();
		
	}
	private void ShowOrHideInteractPrompt()
	{
		int num_collisions = interactionShape.GetCollisionCount();
		List<IInteractable> interactables_this_frame = new List<IInteractable>();
		for (int i = 0; i < num_collisions; i++)
		{
			IInteractable interactable = interactionShape.GetCollider(i) as IInteractable;
			if(interactable != null)
			{
				
				if(interactable.CanInteract(this))
				{
					interactables_this_frame.Add(interactable);
					interactHudElement.Show();
				}
				
			}
		}
		if(interactables_this_frame.Count < 1)
		{
			SetPromptText(interactHudElement.defaultInteractionText);
			interactHudElement.Hide();
			
		}
	}
	private void ProcessInteractionCollisions()
	{
		int num_collisions = interactionShape.GetCollisionCount();
		for (int i = 0; i < num_collisions; i++)
		{
			IInteractable interactable = interactionShape.GetCollider(i) as IInteractable;
			if(interactable != null)
			{
				if(interactable.CanInteract(this))
				{
					EmitSignal(SignalName.ev_ThisColliderInteractedWith, (interactionShape.GetCollider(i) as Node).Owner);
					interactable.OnInteract(this);
				}
			}
		}
	}

	public void SetPromptText(string new_text)
	{
		interactHudElement.SetPromptText(new_text);
	}
	

	

    
}
