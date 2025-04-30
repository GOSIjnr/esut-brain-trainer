class_name vocabularyManager
extends MarginContainer

var libary: Array
var libaryPath: String = "res://Resources/Vocabulary/"

@onready var timer: Timer = $Timer
@onready var word: Label = %Word
@onready var meaning: Label = %Meaning

func _ready() -> void:
	libary = Utils.loadAllResources(libaryPath)
	randomLibary()

func _on_timer_timeout() -> void:
	randomLibary()

func randomLibary() -> void:
	var selected: Vocabulary = libary.pick_random()
	word.text = selected.word
	meaning.text = selected.meaning
