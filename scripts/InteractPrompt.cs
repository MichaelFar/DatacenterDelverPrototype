using Godot;
using System;

public partial class InteractPrompt : RichTextLabel
{
	// Called when the node enters the scene tree for the first time.
	[Export]public string defaultInteractionText = "Interact";
	public override void _Ready()
	{
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}

	public void SetPromptText(string new_text)
	{
		Text = "E" + "\n" + new_text; 
	}
}
