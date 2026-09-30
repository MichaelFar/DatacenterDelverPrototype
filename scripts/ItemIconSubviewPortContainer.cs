using Godot;
using System;
using UsefulDataTypes;
public partial class ItemIconSubviewPortContainer : SubViewportContainer
{
	// Called when the node enters the scene tree for the first time.
	private Node itemSubviewportParent;
	[Export] ItemType itemType;
	public override void _Ready()
	{
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}

	public void HandleItemPickupIcon(Node incoming_scene_root)
	{
		ItemPickup potential_pickup = incoming_scene_root as ItemPickup;
		if(potential_pickup != null)
		{
			if(potential_pickup.GetItemAnimationData().itemType == itemType)
			{
				ItemIconViewport item_icon_viewport = potential_pickup.GetItemIconViewport() as ItemIconViewport;
				if(item_icon_viewport != null)
				{
					itemSubviewportParent = item_icon_viewport.GetParent();
					item_icon_viewport.Reparent(this);
					item_icon_viewport.ShowIconMesh();
					potential_pickup.ev_Dropped += () => {item_icon_viewport.Reparent(itemSubviewportParent);item_icon_viewport.HideIconMesh();};
				}
			}
				
		}
	}
}
