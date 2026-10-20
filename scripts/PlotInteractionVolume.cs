using Godot;
using System;

public partial class PlotInteractionVolume : Area3D, IPlotInteracter
{
	// Called when the node enters the scene tree for the first time.

	[Signal]public delegate void ev_EncounteredPlotEventHandler(FarmPlot interacted_plot);
	[Signal]public delegate void ev_PlotInteractedEventHandler();
	public override void _Ready()
	{
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		
	}

    public void PlotInteract(FarmPlot interacted_plot)
    {
		GD.Print("Running plot interact");
        EmitSignal(SignalName.ev_EncounteredPlot, interacted_plot);
		EmitSignal(SignalName.ev_PlotInteracted);

    }

}
