using Godot;
using System;

public interface IUseItem
{
    public void UsePrimaryMode();
    public void UseAlternateMode();

    
    public void ProcessInput(float delta);
}
