using Godot;
using System;
using System.ComponentModel;
using System.Collections.Generic;

public partial class InteractionArea : Area3D, IInteractable
{
	// Called when the node enters the scene tree for the first time.
    [Signal]
    public delegate void ev_OnInteractEventHandler();

    [Export]
    public string interactText = "Test interact text";

    [Export]
    public bool canInteract = true;
    
	public override void _Ready()
	{
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}

    public void OnInteract(InteractionHandler interaction_source)
    {
        GD.Print("Interacted with object");
        EmitSignal(SignalName.ev_OnInteract);
    }

    public bool CanInteract(InteractionHandler interaction_source)
    {
        if(canInteract)
        {
            interaction_source.SetPromptText(interactText);
        }
            
        return canInteract;
    }
    public void SetCanInteract(bool new_value)
    {
        canInteract = new_value;
    }
    public void SetCanInteractToFalse()
    {
        SetCanInteract(false);
    }
    public void SetCanInteractToTrue()
    {
        SetCanInteract(true);
    }

}
