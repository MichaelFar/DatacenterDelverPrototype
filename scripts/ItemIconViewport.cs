using Godot;
using System;
using System.ComponentModel;

public partial class ItemIconViewport : SubViewport
{
	[Export]private MeshInstance3D iconMeshNode;
	[Export]private Mesh meshOverride;

	[Export]private DirectionalLight3D renderLight;
	[Export]private Camera3D renderCamera;
	[Export(PropertyHint.Layers3DRender)]private uint render_layer;

	public uint originalRenderLayer;
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		originalRenderLayer = render_layer;
		SetRenderLayers(render_layer);
		if(meshOverride != null)
		{
			iconMeshNode.Mesh = meshOverride;
		}
		HideIconMesh();
	}

	public void SetRenderLayers(uint new_layer)
	{
		renderCamera.CullMask = new_layer;
		iconMeshNode.Layers = new_layer;
		renderLight.Layers = new_layer;
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
