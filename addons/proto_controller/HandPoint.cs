using Godot;
using System;

public partial class HandPoint : MeshInstance3D
{
	// Called when the node enters the scene tree for the first time.
	[Export] CharacterBody3D playerController;



	public Node3D weapon;



	Vector3 lastFrameGlobalPosition;

	float originalPositionY = 0.0f;

	int rotationSpeed = 4;

	int frameCounter = 0;

	Vector2 mouseMovement;

	int bobDirection = -1;

	bool isBobbing = false;

	Tween globalTween;

public override void _Ready()
{
	lastFrameGlobalPosition = weapon.GlobalPosition;
	if(playerController.HasSignal("Moving"))
	{
		playerController.Moving.connect(bob_up_and_down);
	}
	
	originalPositionY = position.y;
}
func _ready() -> void:
	
	
	
func _physics_process(delta: float) -> void:
	
	sway_weapon(delta)
	
func _input(event: InputEvent) -> void:
	
	if(event is InputEventMouseMotion && Input.mouse_mode == Input.MouseMode.MOUSE_MODE_CAPTURED):
	
		mouseMovement = event.relative
	
	if(event.is_action_released("primary")):
		
		weapon.activate_primary_strategy("primary")
	
	if(event.is_action_pressed("secondary")):
		
		weapon.activate_secondary_strategy("secondary")

		
func sway_weapon(delta):
	
	mouseMovement = mouseMovement.clamp(weapon.swayMin, weapon.swayMax)
	weapon.position.x = lerp(weapon.position.x, position.x - (mouseMovement.x * weapon.xWeaponSway) * delta, weapon.xWeaponSwayPosition)
	weapon.position.y = lerp(weapon.position.y, position.y + (mouseMovement.y * weapon.yWeaponSway) * delta, weapon.yWeaponSwayPosition)
	
	weapon.rotation_degrees.x = lerp(weapon.rotation_degrees.x, rotation_degrees.x - (mouseMovement.y * weapon.xWeaponRotation) * delta, weapon.xWeaponRotationPosition)
	weapon.rotation_degrees.y = lerp(weapon.rotation_degrees.y, rotation_degrees.y + (mouseMovement.x * weapon.yWeaponRotation) * delta, weapon.yWeaponRotationPosition)

func bob_up_and_down():
	
	if(globalTween != null):
		
		if(globalTween.is_running()):
			
			globalTween.kill()
			returnToOriginalY()
			return
			
	if(!playerController.isMoving):
		returnToOriginalY()
		return
		
	bobDirection *= -1
	var tween = get_tree().create_tween()
	globalTween = tween
	tween.set_trans(Tween.TRANS_QUAD)
	tween.tween_property(self, "position:y", position.y + (.05 * bobDirection), 0.4)
	tween.finished.connect(bob_up_and_down)
	isBobbing = true
	
func returnToOriginalY():
	
	var tween = get_tree().create_tween()
	tween.set_trans(Tween.TRANS_QUAD)
	tween.tween_property(self, "position:y", originalPositionY, 0.4)
	

}