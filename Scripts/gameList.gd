class_name gameList
extends Control

@export var lockNullGames: bool = true

var Game: Games:
	set = setGame

var parent: Node 

@onready var backGround: TextureRect = %BackGround
@onready var gameIcon: TextureRect = %GameIcon
@onready var gameName: Label = %GameName
@onready var gameType: Label = %GameType
@onready var disabledBG: TextureRect = %Disabled

func setGame(resource :Games)  -> void:
	Game = resource
	self.name = resource.gameName
	backGround.texture = resource.gameBackground
	gameIcon.texture = resource.gameIcon
	gameName.text = resource.gameName
	
	var gameRef = ["writing", "speaking", "reading", "maths", "memory"]
	gameType.text = gameRef[resource.gameType]
	
	if lockNullGames and resource.gameScene == null:
		disabledBG.show()
		backGround.material.set_shader_parameter("mix_percentage", 0.6)

func _on_pressed()  -> void:
	if disabledBG.visible == false:
		parent.showSelectedTab(Game)
