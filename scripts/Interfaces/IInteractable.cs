using Godot;
using System;

interface IInteractable
{
    public void OnInteract(InteractionHandler interaction_source);
    public bool CanInteract(InteractionHandler interaction_source);
}
