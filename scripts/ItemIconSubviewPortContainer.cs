using Godot;
using System;
using UsefulDataTypes;
public partial class ItemIconSubviewPortContainer : SubViewportContainer
{
	// Called when the node enters the scene tree for the first time.
	private Node itemSubviewportParent;
	[Export] ItemType itemType;
	ItemPickup.ev_DroppedEventHandler droppedAction;
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
					if(itemType == ItemType.TOOL)
					{
						item_icon_viewport.SetRenderLayers(4);
					}
					itemSubviewportParent = item_icon_viewport.GetParent();
//					GD.Print(item_icon_viewport.Owner.Name);
					item_icon_viewport.Reparent(this);
					item_icon_viewport.ShowIconMesh();
					
					droppedAction = () => ReparentBackToOriginalItem(item_icon_viewport);
					
					
					potential_pickup.ev_Dropped += droppedAction;
					potential_pickup.ev_Dropped += () => {potential_pickup.ev_Dropped -= droppedAction;};
				}
			}
			
				
		}

	}
	private void ReparentBackToOriginalItem(ItemIconViewport item_icon_viewport)
	{
		item_icon_viewport.Reparent(itemSubviewportParent);
		item_icon_viewport.HideIconMesh();
		item_icon_viewport.SetRenderLayers(item_icon_viewport.originalRenderLayer);
	}
	
}
