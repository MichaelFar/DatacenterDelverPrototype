using Godot;
using System;
using System.ComponentModel;

public partial class FarmPlot : Area3D
{
	// Called when the node enters the scene tree for the first time.
	[Export] private Material wateredMaterial;
	private Material originalMaterial;

	private bool isWatered = false;

	[Export] private MeshInstance3D plotMesh;
	public override void _Ready()
	{
		originalMaterial = plotMesh.Mesh.SurfaceGetMaterial(0);
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}

	public void SetIsWatered(bool new_value)
	{
		GD.Print("Attempting to water");
		isWatered = new_value;
		if(isWatered)
		{
			plotMesh.Mesh.SurfaceSetMaterial(0, wateredMaterial);
			
		}
		else
		{
			plotMesh.Mesh.SurfaceSetMaterial(0, originalMaterial);
		}
		
	}
	
	public void ProcessPlotInteractObject(Area3D incoming_area)
	{
		IPlotInteracter plot_interacter = incoming_area as IPlotInteracter;

		if(plot_interacter != null)
		{
			plot_interacter.PlotInteract(this);
		}
	}

	public bool GetIsWatered()
	{
		return isWatered;
	}
}
