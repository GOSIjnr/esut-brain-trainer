class_name global
extends Node

var isToastActive: bool = false
var selectedGameResource: Games

@export var PQColor: Array[Color]

func _ready() -> void:
	Engine.max_fps = 60
