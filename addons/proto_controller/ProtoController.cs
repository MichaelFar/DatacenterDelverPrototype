using System;
using System.Threading.Tasks;
using Godot;

public sealed partial class ProtoController : CharacterBody3D
{
    
    private static readonly float xLimitLowerBound = Mathf.DegToRad(-85);
    private static readonly float xLimitUpperBound = xLimitLowerBound * -1F;
	[Signal]
    public delegate void ev_MovingEventHandler();
	[Signal]
    public delegate void ev_StoppedEventHandler();
    // careful: ExportGroup groups based on field order (seriously!) => don't reorder fields!
    [Export]
    private bool _canFreeFly;

    [Export]
    private bool _canJump = true;

    [Export]
    private bool _canMove = true;

    [Export]
    private bool _canSprint;

    [Export]
    private bool _hasGravity = true;

    [ExportGroup("Speeds")]
    [Export]
    private float _lockSpeed = 0.002F;

    [Export]
    private float _baseSpeed = 7F;

    [Export]
    private float _jumpVelocity = 4.5F;

    [Export]
    private float _sprintSpeed = 10F;

    [Export]
    private float _freeFlySpeed = 25F;

    [ExportGroup("Input Actions")]
    [Export]
    private StringName _inputLeft = "Left";

    [Export]
    private StringName _inputRight = "Right";

    [Export]
    private StringName _inputForward = "Forward";

    [Export]
    private StringName _inputBack = "Back";

    [Export]
    private StringName _inputJump = "ui_select";

    [Export]
    private StringName _inputSprint = "sprint";

    [Export]
    private StringName _inputFreeFly = "free_fly";

    [Export]private CollisionShape3D _collider;
    private bool _freeFlying;
    [Export]private Node3D _head;
    private Vector2 _lookRotation = Vector2.Zero;
    private bool _mouseCaptured;

	public bool isMoving = false;
    public override void _Ready()
    {
        base._Ready();
        
        
        
        _lookRotation = new Vector2(_head.Rotation.X, Rotation.Y);
        EnsureInputMappings();
    }
    bool firstFrame = true;
    void InitializeGlobalValues()
    {
        GD.Print("Initializing player object");
        GlobalValues.Instance.playerObject = this;
    }

    public override void _PhysicsProcess(double delta)
    {
        base._PhysicsProcess(delta);
        if(mouseMotionRelative == mouseMotionRelativePreviousFrame)
        {
            mouseMotionRelative = Vector2.Zero;
        }
        RotateLook(mouseMotionRelative);
        if(GlobalValues.Instance != null && firstFrame)
        {
            firstFrame = false;
            InitializeGlobalValues();
        }
        if (_canFreeFly && _freeFlying)
        {
            var inputDir = GetInputVector();
            var motion = (_head.GlobalTransform.Basis * new Vector3(inputDir.X, 0, inputDir.Y)).Normalized();
            motion *= _freeFlySpeed * (float) delta;
            MoveAndCollide(motion);

            return;
        }

        if (_hasGravity && !IsOnFloor())
        {
            Velocity += GetGravity() * (float) delta;
        }

        if (_canJump && Input.IsActionJustPressed(_inputJump) && IsOnFloor())
        {
            Velocity = Velocity with { Y = _jumpVelocity };
        }

        if (_canMove)
        {
            var moveSpeed = _canSprint && Input.IsActionPressed(_inputSprint)
                ? _sprintSpeed
                : _baseSpeed;
            var inputDir = GetInputVector();
            var moveDir = (Transform.Basis * new Vector3(inputDir.X, 0, inputDir.Y)).Normalized();
            if (moveDir != Vector3.Zero)
            {
				if(!isMoving)
				{
					EmitSignal(SignalName.ev_Moving);
                    
					isMoving = true;
				}
					
                Velocity = Velocity with
                {
                    X = moveDir.X * moveSpeed,
                    Z = moveDir.Z * moveSpeed
                    
                };
            }
            else
            {
				
			    if(isMoving)
                {
                    isMoving = false;
                    EmitSignal(SignalName.ev_Stopped);
                }
                Velocity = Velocity with
                {
                    X = Mathf.MoveToward(Velocity.X, 0, moveSpeed),
                    Z = Mathf.MoveToward(Velocity.Z, 0, moveSpeed)
                };
            }
        }
        else
        {
            
            Velocity = Velocity with
            {
                X = 0,
                Z = 0
            };
        }

        MoveAndSlide();
        mouseMotionRelativePreviousFrame = mouseMotionRelative;
        return;

        Vector2 GetInputVector() => Input.GetVector(_inputLeft, _inputRight, _inputForward, _inputBack);
    }
    private Vector2 mouseMotionRelative;
    private Vector2 mouseMotionRelativePreviousFrame;
    public override void _UnhandledInput(InputEvent @event)
    {
        base._UnhandledInput(@event);

        if (Input.IsMouseButtonPressed(MouseButton.Left))
        {
            SetMouseCaptured();
        }

        if (Input.IsKeyPressed(Key.Escape))
        {
            SetMouseReleased();
        }

        if (_mouseCaptured && @event is InputEventMouseMotion motionEvent)
        {
            mouseMotionRelative = motionEvent.Relative;
            
        }

        if (_canFreeFly && Input.IsActionJustPressed(_inputFreeFly))
        {
            if (!_freeFlying)
            {
                EnableFreeFly();
            }
            else
            {
                DisableFreeFly();
            }
        }
    }

    private void RotateLook(Vector2 rotationInput)
    {
        var x = _lookRotation.X - rotationInput.Y * _lockSpeed;
        _lookRotation.X = Math.Clamp(x, xLimitLowerBound, xLimitUpperBound);
        _lookRotation.Y -= rotationInput.X * _lockSpeed;

        Transform = Transform with
        {
            Basis = Basis.Identity
        };
        RotateY(_lookRotation.Y);

        _head.Transform = _head.Transform with
        {
            Basis = Basis.Identity
        };
        _head.RotateX(_lookRotation.X);
    }

    private void SetMouseCaptured()
    {
        Input.SetMouseMode(Input.MouseModeEnum.Captured);
        _mouseCaptured = true;
    }

    private void SetMouseReleased()
    {
        Input.SetMouseMode(Input.MouseModeEnum.Visible);
        _mouseCaptured = false;
    }

    private void EnableFreeFly()
    {
        _collider.Disabled = true;
        _freeFlying = true;
        Velocity = Vector3.Zero;
    }

    private void DisableFreeFly()
    {
        _collider.Disabled = false;
        _freeFlying = false;
    }

    private void EnsureInputMappings()
    {
        if (_canMove
            && (!CheckInputMappingConfigured(_inputLeft, "Move Left")
                || !CheckInputMappingConfigured(_inputRight, "Move Right")
                || !CheckInputMappingConfigured(_inputForward, "Move Forward")
                || !CheckInputMappingConfigured(_inputBack, "Move Back")))
        {
            GD.PushWarning("Movement disabled.");
            _canMove = false;
        }

        if (_canJump
            && !CheckInputMappingConfigured(_inputJump, "Jump"))
        {
            GD.PushWarning("Jumping disabled.");
            _canJump = false;
        }

        if (_canSprint
            && !CheckInputMappingConfigured(_inputSprint, "Sprint"))
        {
            GD.PushWarning("Sprinting disabled.");
            _canSprint = false;
        }

        if (_canFreeFly
            && !CheckInputMappingConfigured(_inputFreeFly, "Free Fly"))
        {
            GD.PushWarning("Free flying disabled.");
            _canFreeFly = false;
        }

        return;

        static bool CheckInputMappingConfigured(StringName inputAction, string description)
        {
            if (InputMap.HasAction(inputAction))
            {
                return true;
            }

            GD.PushWarning($"Input action '{description}' not found. Set up '{inputAction}'");

            return false;
        }
    }
}