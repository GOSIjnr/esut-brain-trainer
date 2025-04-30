class_name content3Manager
extends Control

var gameLocation: String = "res://Resources/Games/"
var gameLibary: Array
var gamePlaceHolder: PackedScene = preload("res://Scenes/UI/gameList.tscn")

@onready var gameBar: GridContainer = %GameBar

@export var filterGroup: ButtonGroup
@export var selectedGameTab: PackedScene

func _ready() -> void:
	gameLibary = Utils.loadAllResources(gameLocation)
	
	var setter: Array[BaseButton] = filterGroup.get_buttons()
	for button in setter:
		button.pressed.connect(_on_button_pressed.bind(button, setter.find(button)))
	
	setter[0].emit_signal("pressed")

func _resetLibary() -> void:
	for content in gameBar.get_children():
		content.queue_free()
	
	for button in filterGroup.get_buttons():
		button.disabled = false

func _on_button_pressed(button :Button, index :int) -> void:
	_resetLibary()
	button.disabled = true
	
	for game in gameLibary.size():
		if index == 0:
			var gameScene = gamePlaceHolder.instantiate()
			gameBar.add_child(gameScene)
			gameScene.Game = gameLibary[game]
			gameScene.parent = self
		else:
			if gameLibary[game].gameType == index - 1:
				var gameScene = gamePlaceHolder.instantiate()
				gameBar.add_child(gameScene)
				gameScene.Game = gameLibary[game]
				gameScene.parent = self

func showSelectedTab(Game :Games) -> void:
	var scene = selectedGameTab.instantiate() as selectedGame
	get_tree().root.add_child(scene)
	
	Global.selectedGameResource = Game
	scene.selected = Game
