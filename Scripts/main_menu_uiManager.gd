class_name mainMenuUiManger
extends Control

@onready var tabs: Panel = %Tabs

func _ready() -> void:
	SaveManager.loadData()
	get_tree().quit_on_go_back = false

func _notification(what) -> void:
	match what:
		NOTIFICATION_WM_GO_BACK_REQUEST:
			go_back_request()

func go_back_request() -> void:
	var setter: Array[BaseButton] = tabs.TabGroup.get_buttons()
	var lastNode = get_tree().root.get_children().back()
	
	if setter[0].is_pressed():
		get_tree().quit()
	else:
		if lastNode == self:
			setter[0].emit_signal("pressed")
