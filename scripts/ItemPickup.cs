using Godot;
using System;
using System.ComponentModel;

public partial class ItemPickup : RigidBody3D, IUseItem
{
	// Called when the node enters the scene tree for the first time.
	[Export] private ItemAnimationData itemAnimationData;
	[Export] private InteractionArea myArea;
	
	[Export] private PathFollow3D swayPath;
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

	private float defaultPathFollowProgress;
	private bool swayPathAvailable = false;

	private Vector2 swayModVector = Vector2.Zero;
	private float swayModVectorMagnitude = 0.0f;
	private float deltaCoefficient = 1.0f;
	public override void _Ready()
	{
		if(swayPath != null)
		{
			swayPathAvailable = true;
			defaultPathFollowProgress = swayPath.Progress;
		}
		
		
		//InitializeGlobalValues();
	}
	
	public void InitializeGlobalValues()
    {
        
        //GlobalValues.Instance.playerObject.ev_Moving += bob_up_and_down;
		playerObject = GlobalValues.Instance.playerObject;
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
	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _PhysicsProcess(double delta)
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
			Position = new Vector3(Position.X, (Position.Y + (0.01f * bobDirection)), Position.Z).Lerp(Position, move_ratio);
			if(swayPathAvailable)
			{
				swayPath.Progress = Mathf.Lerp(swayPath.Progress, swayPath.Progress + (bobDirection * 0.002f), move_ratio);
			}
				
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
			if(swayPathAvailable)
			{
				swayPath.Progress = Mathf.Lerp(swayPath.Progress, defaultPathFollowProgress, move_ratio);
			}
			bobDirection = -1;
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
		sway_item(delta, swayModVector, swayModVectorMagnitude);
		originalPositionY = Position.Y;
		
    }
	public void SetSwayMods(Vector2 new_sway_vector, float new_sway_magnitude)
	{
		swayModVector = new_sway_vector;
		swayModVectorMagnitude = new_sway_magnitude;
	}
	public override void _Input(InputEvent @event)
	{
		if(@event is InputEventMouseMotion && Input.MouseMode == Input.MouseModeEnum.Captured)
		{
			InputEventMouseMotion mouse_motion = @event as InputEventMouseMotion;
			mouseMovement = mouse_motion.Relative;
		}
		
	}
	private void sway_item(double delta, Vector2 sway_mod_vector = new Vector2(), float sway_mod_vector_magnitude = 0.0f)
	{
			sway_mod_vector = sway_mod_vector.Normalized() * sway_mod_vector_magnitude;
			ItemAnimationData item_anim_data = GetItemAnimationData();
			mouseMovement = mouseMovement.Clamp(item_anim_data.swayMin, item_anim_data.swayMax);
			var held_item_transform = Transform;
			
			held_item_transform.Origin = new Vector3((float)Mathf.Lerp(Position.X,  swayReferenceObject.Position.X - (/*sway_mod_vector.X*/ +(mouseMovement.X * item_anim_data.xWeaponSway) * delta), 
				item_anim_data.xWeaponSwayPosition), 
				(float)Mathf.Lerp(Position.Y, 
				swayReferenceObject.Position.Y + (/*sway_mod_vector.Y + */(mouseMovement.Y * item_anim_data.yWeaponSway)) * delta, 
				item_anim_data.yWeaponSwayPosition), 0);
			
			Transform = held_item_transform;
			
			RotationDegrees = new Vector3((float)Mathf.Lerp(RotationDegrees.X, 
				swayReferenceObject.RotationDegrees.X - (-sway_mod_vector.X * 250.0f + (mouseMovement.Y * item_anim_data.xWeaponRotation)) *delta, 
				item_anim_data.xWeaponRotationPosition), 
				(float)Mathf.Lerp(RotationDegrees.Y, 
				swayReferenceObject.RotationDegrees.Y + (-sway_mod_vector.Y * 35.0f + (mouseMovement.X * item_anim_data.yWeaponRotation)) * delta, 
			
			item_anim_data.yWeaponRotationPosition), Rotation.Z);
			
	}

	
	
}
