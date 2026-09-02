using Godot;
using System;

public partial class GlobalValues : Node3D
{
    public static GlobalValues Instance { get; private set; }
    [Signal]public delegate void ev_InitializedEventHandler();
    
    public bool playerInitialized = false;

    public override void _Ready()
    {
        Instance = this;
        EmitSignal(SignalName.ev_Initialized);
    }

    public override void _Process(double delta)
    {
        base._Process(delta);
        if(!playerInitialized)
        {
            if(playerObject != null)
            {
                playerInitialized = true;
            }
        }
    }

    
    public ProtoController playerObject;
}
