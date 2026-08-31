using Godot;
using System;

interface IInteractable
{
    public void OnInteract();
    public bool CanInteract();
}
