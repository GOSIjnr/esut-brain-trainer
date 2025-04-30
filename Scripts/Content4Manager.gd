class_name content4Manager
extends Control

@onready var user_name: Label = %UserName
@onready var workout_counter: Label = %WorkoutCounter
@onready var writing_cb: Button = %WritingCB
@onready var speaking_cb: Button = %SpeakingCB
@onready var reading_cb: Button = %ReadingCB
@onready var maths_cb: Button = %MathsCB
@onready var memory_cb: Button = %MemoryCB

@export var deleteAccountScene: PackedScene

func _ready() -> void:
	SaveManager.loadData()
	
	user_name.text = SaveManager.fileData.UserName
	workout_counter.text = str(SaveManager.fileData.Workouts)
	
	writing_cb.button_pressed = SaveManager.fileData.Writing
	speaking_cb.button_pressed = SaveManager.fileData.Speaking
	reading_cb.button_pressed = SaveManager.fileData.Reading
	maths_cb.button_pressed = SaveManager.fileData.Maths
	memory_cb.button_pressed = SaveManager.fileData.Memory

func _on_check_button_1_toggled(toggled_on) -> void:
	SaveManager.fileData.Writing = toggled_on
	SaveManager.saveData()

func _on_check_button_2_toggled(toggled_on) -> void:
	SaveManager.fileData.Speaking = toggled_on
	SaveManager.saveData()

func _on_check_button_3_toggled(toggled_on) -> void:
	SaveManager.fileData.Reading = toggled_on
	SaveManager.saveData()

func _on_check_button_4_toggled(toggled_on) -> void:
	SaveManager.fileData.Maths = toggled_on
	SaveManager.saveData()

func _on_check_button_5_toggled(toggled_on) -> void:
	SaveManager.fileData.Memory = toggled_on
	SaveManager.saveData()

func _on_delete_account_pressed() -> void:
	var scene = deleteAccountScene.instantiate()
	get_tree().root.add_child(scene)
