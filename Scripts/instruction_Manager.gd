class_name instructionManager
extends Control

@onready var instructions: RichTextLabel = %Instructions
@onready var ui: Control = %UI

signal instructionClosed

func _ready() -> void:
	var resource: Games = Global.selectedGameResource
	
	if not resource == null:
		updateText(instructions, "[b]INSTRUCTIONS[/b]", resource.howToPlay, "")

func updateText(text :RichTextLabel, suffix :String, textToAdd :String, prefix :String) -> void:
	text.clear()
	text.append_text(suffix)
	text.add_text("\n\n")
	text.append_text(textToAdd)
	
	if not prefix.is_empty():
		text.add_text("\n\n")
		text.append_text(prefix)

func _on_continue_pressed() -> void:
	self.hide()
	ui.show()
	get_tree().paused = false
	instructionClosed.emit()
