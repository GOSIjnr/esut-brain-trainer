class_name global
extends Node

var isToastActive: bool = false
var selectedGameResource: Games

@export var PQColor: Array[Color] = [
	Color("#2BBFA9"),
	Color("#EF2430"),
	Color("#EC1A61"),
	Color("#9C29A8"),
	Color("#E79220"),
	Color("#69A0FB"),
]


func _ready() -> void:
	Engine.max_fps = 60
