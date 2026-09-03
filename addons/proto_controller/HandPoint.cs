using Godot;
using System;
using System.Runtime.CompilerServices;

public partial class HandPoint : MeshInstance3D
{
	// Called when the node enters the scene tree for the first time.
	[Export] ProtoController playerController;



	[Export]public ItemPickup heldItem;

	private bool isOccupied = false;

	Vector3 lastFrameGlobalPosition;

	float originalPositionY = 0.0f;

	int rotationSpeed = 4;

	int frameCounter = 0;

	Vector2 mouseMovement;

	int bobDirection = -1;

	bool isBobbing = false;

	Tween globalTween;
	
	public override void _Ready()
	{
		lastFrameGlobalPosition = GlobalPosition;
		
		//playerController.ev_Moving += bob_up_and_down;
		
		
		originalPositionY = Position.Y;
	}

		
		
		
	public override void _PhysicsProcess(double delta)
	{
		//sway_weapon(delta);
		if(Input.IsActionJustReleased("DropItem"))
		{
			DropItem();
		}
		if(heldItem != null)
		{
			heldItem.ProcessInput((float)delta);
		}
		
		
		
	}
		

	public override void _Input(InputEvent @event)
	{
		if(@event is InputEventMouseMotion && Input.MouseMode == Input.MouseModeEnum.Captured)
		{
			InputEventMouseMotion mouse_motion = @event as InputEventMouseMotion;
			mouseMovement = mouse_motion.Relative;
		}
		
			
		
	}


	public void ProcessPotentialPickup(Node3D item_to_check)
	{
		ItemPickup test_pickup = item_to_check as ItemPickup;
		GD.Print("Attempting to pickup item " + item_to_check);
		if(test_pickup != null)
		{
			
			PickupItem(test_pickup);
		}
	}
	private void PickupItem(ItemPickup item_to_pickup)
	{
		if(heldItem != null)
		{
			DropItem();
		}
		heldItem = item_to_pickup;
		heldItem.Reparent(this);
		//GlobalValues.Instance.playerObject.ev_Moving += heldItem.bob_up_and_down;
		
		heldItem.swayReferenceObject = this;
		heldItem.BecomeHeld();
		heldItem.GlobalPosition = GlobalPosition;
		
		//heldItem.Transform = Transform;
	}

	public void DropItem()
	{
		if(heldItem == null)
		{
			return;
		}
		//GlobalValues.Instance.playerObject.ev_Moving -= heldItem.bob_up_and_down;
		heldItem.BecomeDropped();
		heldItem.isHeld = false;
		heldItem.SetFreeze(false);
		heldItem.Reparent(GetTree().Root);
		heldItem.SetCanInteract(true);
		heldItem = null;
		//heldItem.GlobalPosition = GlobalPosition;
	}
	
	
	

}
