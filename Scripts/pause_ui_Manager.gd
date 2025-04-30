class_name pauseUiManager
extends CanvasLayer

func _notification(what) -> void:
	match what:
		NOTIFICATION_WM_GO_BACK_REQUEST:
			_on_continue_pressed()

func _on_continue_pressed() -> void:
	get_tree().paused = false
	self.hide()

func _on_quit_pressed() -> void:
	get_tree().paused = false
	get_tree().change_scene_to_packed(SceneLoader.get_resource("main_menu"))
