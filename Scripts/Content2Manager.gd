class_name content2Manager
extends Control

@export var help: PackedScene
@export var AnalyticGroup: ButtonGroup

@onready var analytics: Control = %Analytics as analyticsChart
@onready var epq_holder: VBoxContainer = %"EPQ Holder"

func _ready() -> void:
	for epq in epq_holder.get_children():
		if epq is EPQ:
			epq.progressBarColor = Global.PQColor[epq.get_index()]
	
	var setter: Array[BaseButton] = AnalyticGroup.get_buttons()	
	for button in setter:
		button.modulate = Global.PQColor[setter.find(button)]
		button.pressed.connect(_on_button_pressed.bind(button))
	
	setter.front().emit_signal("pressed")

func _on_help_gui_input(event) -> void:
	if event is InputEventScreenTouch and event.is_released():
		var rect = Rect2(Vector2(0, 0), Vector2(100, 100))
		
		if rect.has_point(event.position) and not help == null:
			var scene = help.instantiate()
			get_tree().root.add_child(scene)

func _resetLibary() -> void:
	for button in AnalyticGroup.get_buttons():
		button.disabled = false
		button.text = button.name.substr(0, 1)

func _on_button_pressed(button: Button) -> void:
	_resetLibary()
	var space = "     "
	button.text = space + button.name + space
	button.disabled = true
	
	var ref = analytics as analyticsChart
	ref.type = button.name
