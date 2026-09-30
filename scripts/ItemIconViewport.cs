using Godot;
using System;

public partial class ItemIconViewport : SubViewport
{
	[Export]private MeshInstance3D iconMeshNode;
	[Export]private Mesh meshOverride;
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		if(meshOverride != null)
		{
			iconMeshNode.Mesh = meshOverride;
		}
		HideIconMesh();
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}
	public void ShowIconMesh()
	{
		iconMeshNode.Show();
	}
	public void HideIconMesh()
	{
		iconMeshNode.Hide();
	}
}
