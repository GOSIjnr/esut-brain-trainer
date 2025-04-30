class_name tabManager
extends Panel

var contentID: Array[String] = ["content_1", "content_2", "content_3", "content_4"]

@onready var contentTab: Control = %Contents

@export var TabGroup: ButtonGroup
@export var TabColor_Selected: Color
@export var TabColor_Deselected: Color
@export var boldFont: FontFile

@onready var setter: Array[BaseButton] = TabGroup.get_buttons()

func _ready() -> void:
	for tab in setter:
		tab.pressed.connect(_on_button_pressed.bind(tab, setter.find(tab)))
		
		var label: Label = tab.get_parent().find_child("Label")
		label.gui_input.connect(_on_label_pressed.bind(setter.find(tab)))
	
	setter[0].emit_signal("pressed")

func _resetTabs() -> void:
	for child in contentTab.get_children():
		child.queue_free()
	
	for tab in TabGroup.get_buttons():
		tab.disabled = false
		tab.get_parent().modulate = TabColor_Deselected
		
		var label: Label = tab.get_parent().find_child("Label")
		label.remove_theme_font_override("font")

func _on_button_pressed(button: TextureButton, index: int) -> void:
	_resetTabs()
	
	var label: Label = button.get_parent().find_child("Label")
	label.add_theme_font_override("font", boldFont)
	
	if not button.is_pressed():
		button.button_pressed = true
	
	button.disabled = true
	button.get_parent().modulate = TabColor_Selected
	
	var scene: String = contentID[index]
	var content = SceneLoader.get_resource(scene).instantiate()
	contentTab.add_child(content)

func _on_label_pressed(event: InputEvent, index: int) -> void:
	if event is InputEventScreenTouch and event.is_pressed():
		setter[index].emit_signal("pressed")
