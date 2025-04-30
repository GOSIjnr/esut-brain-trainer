class_name playerAgilty
extends CharacterBody2D

@export var gravity: Vector2 = Vector2(0, 15)
var steering: bool = false

const SPEED = 300.0
const JUMP_VELOCITY = -800

@onready var audioPlayer: AudioStreamPlayer = $AudioStreamPlayer
@export var backgroundParticle: CPUParticles2D

signal cancelTimer

func _physics_process(delta: float) -> void:
	velocity += gravity * delta
	move_and_slide()

func _on_game_ui_boost_space_ship() -> void:
	_jump()

func _jump() -> void:
	cancelTimer.emit()
	velocity.y = JUMP_VELOCITY
	
	if steering:
		velocity.x = randf_range(50, 200)
		steering = false
	else:
		velocity.x = randf_range(-200, -50)
		steering = true
	
	audioPlayer.play()
	
	var tween1 = create_tween()
	tween1.tween_property(backgroundParticle, "gravity", Vector2(0, 500), 0.5)
	await get_tree().create_timer(0.5).timeout
	var tween2 = create_tween()
	tween2.tween_property(backgroundParticle, "gravity", Vector2(0, 90), 1)
