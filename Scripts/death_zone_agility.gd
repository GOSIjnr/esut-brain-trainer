class_name deathZoneAgility
extends Area2D

@onready var ca_shader: Panel = %"CA shader"
@onready var timer: Timer = $Timer
@onready var audioPlayer: AudioStreamPlayer = $AudioStreamPlayer

var elsapedTime: float = 0.0

signal playerOutofBound

func _process(delta: float) -> void:
	if ca_shader.visible == true:
		var time = (timer.wait_time - timer.time_left) * 2
		
		elsapedTime += delta
		
		if timer.is_stopped():
			ca_shader.material.set_shader_parameter("r_displacement", Vector2(0, 0))
			ca_shader.material.set_shader_parameter("b_displacement", Vector2(0, 0))
			elsapedTime = 0.0
		else:
			if audioPlayer.playing == false and elsapedTime >= 2.0:
				audioPlayer.play()
				elsapedTime = 0.0
			
			ca_shader.material.set_shader_parameter("r_displacement", Vector2(time, 0))
			ca_shader.material.set_shader_parameter("b_displacement", Vector2(-time, 0))

func _on_body_entered(_body: Node2D) -> void:
	audioPlayer.play()
	timer.start()

func _on_body_exited(_body: Node2D) -> void:
	timer.stop()

func _on_timer_timeout() -> void:
	ca_shader.hide()
	playerOutofBound.emit()

func _on_player_cancel_timer() -> void:
	timer.stop()
