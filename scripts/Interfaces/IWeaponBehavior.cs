using Godot;
using System;

public interface IWeaponBehavior
{
    public bool CanShoot();
    public void Shoot();

    public void Reload();

    
}
