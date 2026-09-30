using Godot;
using System;
using UsefulDataTypes;
namespace UsefulDataTypes
{
    public enum ItemType {WEAPON, TOOL};
}
[GlobalClass]
public partial class ItemAnimationData : Resource
{
    [Export(PropertyHint.Range, "-0.0,1.0,0.1")]public float yWeaponSway;
    [Export(PropertyHint.Range, "-0.0,1.0,0.1")] public float xWeaponSway;
    [Export(PropertyHint.Range, "-0.0,1.0,0.1")] public float yWeaponSwayPosition;
    [Export(PropertyHint.Range, "-0.0,1.0,0.1")] public float xWeaponSwayPosition;
    [Export(PropertyHint.Range, "-0.0,100.0,0.1")] public float yWeaponRotation;
    [Export(PropertyHint.Range, "-0.0,100.0,0.1")] public float xWeaponRotation;
    [Export(PropertyHint.Range, "-0.0,1.0,0.1")] public float yWeaponRotationPosition;
    [Export(PropertyHint.Range, "-0.0,1.0,0.1")] public float xWeaponRotationPosition;
    [Export]public Vector2 swayMin = new Vector2(-20,-20);
    [Export]public Vector2 swayMax = new Vector2(20,20);

    

    [Export] public ItemType itemType = ItemType.WEAPON;
}
