using Godot;
using System;
[GlobalClass]
public partial class WeaponData : Resource
{
    [Export] public int maxAmmoInMagazine = 10;
	[Export] public int currentAmmoInMagazine = 0;
	[Export] public int totalAmmo = 50;
    [Export] public float weaponSpread = 0.0f;
    [Export] public float reloadTime = 1.0f;
    [Export] public PackedScene projectileScene;
    
    [Export] public e_FireType fireMode = e_FireType.Semiautomatic; 

    [Export] public float fireCooldown = 0.05f;

    [Export] public float recoil = 3.0f;

    [Export] public CurveTexture recoilCurve;

    
    public enum e_FireType {Automatic, Semiautomatic};
}
