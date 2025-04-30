class_name correctionManager
extends Control

@onready var correction_panel: Control = %"Correction Panel"
@onready var correction_box: Panel = %"Correction Box"
@onready var correction_text: RichTextLabel = %"Correction Text"
@onready var animationPlayer: AnimationPlayer = $AnimationPlayer

var word: String
var option1: String
var option2: String
var option1Meaning: String
var option2Meaning: String

var correction: String = "The correct answer is [option1]
\n[option1] means [option1Meaning], which means the same thing as [word]
\n[i]while[/i]
\n[option2] means [option2Meaning]
\n[center][i]Tap to continue[/i][/center]"

signal correctionDone

func _ready() -> void:
	self.visibility_changed.connect(updateUI)

func _input(event: InputEvent) -> void:
	if event is InputEventScreenTouch and event.is_released() and self.visible == true:
		if not animationPlayer.is_playing():
			correction_text.hide()
			animationPlayer.play("Panel_Close")
			await animationPlayer.animation_finished
			get_tree().paused = false
			self.hide()
			correctionDone.emit()

func updateUI() -> void:
	if self.visible == true:
		correction_text.text = updateCorrection()
		correction_text.hide()
		animationPlayer.play("Panel_Open")
		await animationPlayer.animation_finished
		correction_text.show()

func updateCorrection() -> String:
	var replacement: Dictionary = {
		"[word]": "[b]" + word + "[/b]",
		"[option1]": "[b]" + option1 + "[/b]",
		"[option2]": "[b]" + option2 + "[/b]",
		"[option1Meaning]": "[b]" + option1Meaning + "[/b]",
		"[option2Meaning]": "[b]" + option2Meaning + "[/b]",
	}
	
	var mod_correction: String = correction
	
	for keyword in replacement.keys():
		mod_correction = mod_correction.replace(keyword, replacement[keyword])
	
	return mod_correction
