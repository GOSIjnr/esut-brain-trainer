class_name deleteAccount
extends Control

func _on_yes_pressed() -> void:
	SaveManager.newData()
	get_tree().change_scene_to_packed(SceneLoader.get_resource("start_up"))

func _on_no_pressed() -> void:
	self.queue_free()

func _notification(what) -> void:
	match what:
		NOTIFICATION_WM_GO_BACK_REQUEST:
			self.queue_free()
