using Godot;
using System;

public partial class PDAManager : Node3D
{
	// Called when the node enters the scene tree for the first time.
	private bool isHeld = false;

	[Export] Control[] uiScreenArray;

	private int currentScreenIndex = 0;
	public override void _Ready()
	{
		ShowNextScreenAlone(-1);
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		if(Input.IsActionJustReleased("CyclePDAScreens") && isHeld)
		{
			currentScreenIndex = ShowNextScreenAlone(currentScreenIndex);
		}
	}
	public void SetIsHeld(bool new_value)
	{
		isHeld = new_value;
	}

	private int ShowNextScreenAlone(int screen_index)
	{
		foreach (Control i in uiScreenArray)
		{
			i.Hide();
		}
		int wrap_index = Mathf.Wrap(screen_index + 1, 0, uiScreenArray.Length);
		uiScreenArray[wrap_index].Show();
		return wrap_index;
	}
}
