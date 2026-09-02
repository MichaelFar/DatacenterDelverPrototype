using Godot;
using System;
using System.ComponentModel;

public partial class ItemPickup : RigidBody3D, IUseItem
{
	// Called when the node enters the scene tree for the first time.
	[Export] private ItemAnimationData itemAnimationData;
	[Export] private InteractionArea myArea;
	
    [Signal]public delegate void ev_UsedPrimaryEventHandler();
	[Signal]public delegate void ev_UsedAlternateEventHandler();

	[Signal]public delegate void ev_ProcessInputEventHandler(float delta);

	Vector2 mouseMovement;
	Tween globalMovementTween;

	int bobDirection = 1;
	public Node3D swayReferenceObject;
	public bool isHeld = false;
	ProtoController playerObject;

	bool firstFrame = true;
	private float originalPositionY;

	[Export] float timeToBob = 0.4f;
	private float moveTimer = 0.0f;
	public override void _Ready()
	{
		
		
		//InitializeGlobalValues();
	}
	public void BecomeHeld()
	{
		
		SetFreeze(true);
		
		isHeld = true;
		
	}
	public void BecomeDropped()
	{
		moveTimer = 0.0f;
	}
	public void InitializeGlobalValues()
    {
        
        //GlobalValues.Instance.playerObject.ev_Moving += bob_up_and_down;
		playerObject = GlobalValues.Instance.playerObject;
    }

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		if(GlobalValues.Instance.playerInitialized && firstFrame)
        {
            firstFrame = false;
            InitializeGlobalValues();
        }
		if(isHeld && playerObject.isMoving)
		{
			moveTimer += (float)delta;
			float move_ratio = moveTimer / timeToBob;
			Position = new Vector3(Position.X, (float)(Position.Y + (0.01 * bobDirection)), Position.Z).Lerp(Position, move_ratio);
			if(moveTimer >= timeToBob)
			{
				moveTimer = 0.0f;
				bobDirection *= -1;
			}
		}
		else if(isHeld)
		{
			moveTimer += (float)delta;
			float move_ratio = moveTimer / timeToBob;
			Position = new Vector3(Position.X, (float)(originalPositionY), Position.Z).Lerp(Position, move_ratio);
		}
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

    public void UsePrimaryMode()
    {
        GD.Print("Primary Mode Used");
		EmitSignal(SignalName.ev_UsedPrimary);
    }

    public void UseAlternateMode()
    {
        GD.Print("Alternate Mode Used");
		EmitSignal(SignalName.ev_UsedAlternate);
    }

    public void ProcessInput(float delta)
    {
        EmitSignal(SignalName.ev_ProcessInput, delta);
		sway_item(delta);
		originalPositionY = Position.Y;
		
    }
	public override void _Input(InputEvent @event)
	{
		if(@event is InputEventMouseMotion && Input.MouseMode == Input.MouseModeEnum.Captured)
		{
			InputEventMouseMotion mouse_motion = @event as InputEventMouseMotion;
			mouseMovement = mouse_motion.Relative;
		}
		
	}
	private void sway_item(double delta)
	{
		
			ItemAnimationData item_anim_data = GetItemAnimationData();
			mouseMovement = mouseMovement.Clamp(item_anim_data.swayMin, item_anim_data.swayMax);
			var held_item_transform = Transform;
			held_item_transform.Origin = new Vector3((float)Mathf.Lerp(Position.X,  swayReferenceObject.Position.X - (mouseMovement.X * item_anim_data.xWeaponSway) * delta, 
			item_anim_data.xWeaponSwayPosition)
			, (float)Mathf.Lerp(Position.Y, 
			swayReferenceObject.Position.Y + (mouseMovement.Y * item_anim_data.yWeaponSway) * delta, 
			item_anim_data.yWeaponSwayPosition), 0);
			Transform = held_item_transform;
			//Position.X=Mathf.Lerp(position.x, Position.X - (mouseMovement.x * weapon.xWeaponSway) * delta, weapon.xWeaponSwayPosition);
			//Position.X = Mathf.Lerp(position.x, Position.X - (mouseMovement.x * weapon.xWeaponSway) * delta, weapon.xWeaponSwayPosition);
			//weapon.position.y = lerp(weapon.position.y, position.y + (mouseMovement.y * weapon.yWeaponSway) * delta, weapon.yWeaponSwayPosition);
			RotationDegrees = new Vector3((float)Mathf.Lerp(RotationDegrees.X, 
			swayReferenceObject.RotationDegrees.X - (mouseMovement.Y * item_anim_data.xWeaponRotation) * delta, 
			item_anim_data.xWeaponRotationPosition), 
			(float)Mathf.Lerp(RotationDegrees.Y, 
			swayReferenceObject.RotationDegrees.Y + (mouseMovement.X * item_anim_data.yWeaponRotation) * delta, 
			item_anim_data.yWeaponRotationPosition), Rotation.Z);
			//RotationDegrees.X =
			//weapon.rotation_degrees.x = lerp(weapon.rotation_degrees.x, rotation_degrees.x - (mouseMovement.y * weapon.xWeaponRotation) * delta, weapon.xWeaponRotationPosition);
			//weapon.rotation_degrees.y = lerp(weapon.rotation_degrees.y, rotation_degrees.y + (mouseMovement.x * weapon.yWeaponRotation) * delta, weapon.yWeaponRotationPosition);
		
			
	}

	
	public void bob_up_and_down()
	{
		
		if(globalMovementTween != null)
		{
			if(globalMovementTween.IsRunning())
			{
				GD.Print("Killing tween");
				globalMovementTween.Kill();
				ReturnToOriginalY();
				return;
			}
		}
		
			
		if(!playerObject.isMoving)
		{
			GD.Print("Returning to original y position");
			ReturnToOriginalY();
			return;
		}
			
		GD.Print("Bobbing");
		bobDirection *= -1;
		var tween = GetTree().CreateTween();
		globalMovementTween = tween;
		tween.SetTrans(Tween.TransitionType.Quad);
		tween.TweenProperty(swayReferenceObject, "position:y", Position.Y + (.05 * bobDirection), 0.4);
		tween.Finished += bob_up_and_down;
			
		
	}
	private void ReturnToOriginalY()
	{
		var tween = GetTree().CreateTween();
		tween.SetTrans(Tween.TransitionType.Quad);
		tween.TweenProperty(this, "position:y", originalPositionY, 0.4);
	}
}
