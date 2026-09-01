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
		
		playerController.ev_Moving += bob_up_and_down;
		
		
		originalPositionY = Position.Y;
	}

		
		
		
	public override void _PhysicsProcess(double delta)
	{
		sway_weapon(delta);
		if(Input.IsActionJustReleased("DropItem"))
		{
			DropItem();
		}
	}
		

	public override void _Input(InputEvent @event)
	{
		if(@event is InputEventMouseMotion && Input.MouseMode == Input.MouseModeEnum.Captured)
		{
			InputEventMouseMotion mouse_motion = @event as InputEventMouseMotion;
			mouseMovement = mouse_motion.Relative;
		}
		
			
		/*
		if(event.is_action_released("primary")):
			
			weapon.activate_primary_strategy("primary")
		
		if(event.is_action_pressed("secondary")):
			
			weapon.activate_secondary_strategy("secondary")
			*/
	}



	private void sway_weapon(double delta)
	{
		if(heldItem != null)
		{
			ItemAnimationData item_anim_data = heldItem.GetItemAnimationData();
			mouseMovement = mouseMovement.Clamp(item_anim_data.swayMin, item_anim_data.swayMax);
			var held_item_transform = heldItem.Transform;
			held_item_transform.Origin = new Vector3((float)Mathf.Lerp(heldItem.Position.X, Position.X - (mouseMovement.X * item_anim_data.xWeaponSway) * delta, 
			item_anim_data.xWeaponSwayPosition)
			, (float)Mathf.Lerp(heldItem.Position.Y, 
			Position.Y + (mouseMovement.Y * item_anim_data.yWeaponSway) * delta, 
			item_anim_data.yWeaponSwayPosition), 0);
			heldItem.Transform = held_item_transform;
			//heldItem.Position.X=Mathf.Lerp(heldItem.position.x, Position.X - (mouseMovement.x * weapon.xWeaponSway) * delta, weapon.xWeaponSwayPosition);
			//heldItem.Position.X = Mathf.Lerp(heldItem.position.x, Position.X - (mouseMovement.x * weapon.xWeaponSway) * delta, weapon.xWeaponSwayPosition);
			//weapon.position.y = lerp(weapon.position.y, position.y + (mouseMovement.y * weapon.yWeaponSway) * delta, weapon.yWeaponSwayPosition);
			heldItem.RotationDegrees = new Vector3((float)Mathf.Lerp(heldItem.RotationDegrees.X, 
			RotationDegrees.X - (mouseMovement.Y * item_anim_data.xWeaponRotation) * delta, 
			item_anim_data.xWeaponRotationPosition), 
			(float)Mathf.Lerp(heldItem.RotationDegrees.Y, 
			RotationDegrees.Y + (mouseMovement.X * item_anim_data.yWeaponRotation) * delta, 
			item_anim_data.yWeaponRotationPosition), heldItem.Rotation.Z);
			//heldItem.RotationDegrees.X =
			//weapon.rotation_degrees.x = lerp(weapon.rotation_degrees.x, rotation_degrees.x - (mouseMovement.y * weapon.xWeaponRotation) * delta, weapon.xWeaponRotationPosition);
			//weapon.rotation_degrees.y = lerp(weapon.rotation_degrees.y, rotation_degrees.y + (mouseMovement.x * weapon.yWeaponRotation) * delta, weapon.yWeaponRotationPosition);
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
			return;
		}
		heldItem = item_to_pickup;
		heldItem.SetFreeze(true);
		heldItem.Reparent(this);
		heldItem.GlobalPosition = GlobalPosition;
		
		//heldItem.Transform = Transform;
	}

	public void DropItem()
	{
		if(heldItem == null)
		{
			return;
		}
		
		heldItem.SetFreeze(false);
		heldItem.Reparent(GetTree().Root);
		heldItem.SetCanInteract(true);
		heldItem = null;
		//heldItem.GlobalPosition = GlobalPosition;
	}
	public void bob_up_and_down()
	{
		
		if(globalTween != null)
		{
			if(globalTween.IsRunning())
			{
				globalTween.Kill();
				ReturnToOriginalY();
				return;
			}
		}
				
		if(!playerController.isMoving)
		{
			ReturnToOriginalY();
			return;
		}
			
			
		bobDirection *= -1;
		var tween = GetTree().CreateTween();
		globalTween = tween;
		tween.SetTrans(Tween.TransitionType.Quad);
		tween.TweenProperty(this, "position:y", Position.Y + (.05 * bobDirection), 0.4);
		tween.Finished += bob_up_and_down;
		isBobbing = true;
	}
	private void ReturnToOriginalY()
	{
		var tween = GetTree().CreateTween();
		tween.SetTrans(Tween.TransitionType.Quad);
		tween.TweenProperty(this, "position:y", originalPositionY, 0.4);
	}
	
	

}
