class_name content1Manager
extends Control

func _on_recommended_button_pressed() -> void:
	Utils.showToast(get_tree().root, "No recommendation found", 1.5)
