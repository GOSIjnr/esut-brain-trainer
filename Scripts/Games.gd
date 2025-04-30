class_name Games
extends Resource

@export_group("Game")
@export var gameColor: Color

@export var gameName: String
@export var gameIcon: Texture2D
@export var gameBackground: Texture2D
@export var gameFullBackground: Texture2D

enum Type {writing, speaking, reading, maths, memory}
@export var gameType: Type

@export_group("Extras")
@export_multiline var howToPlay: String
@export_multiline var gameBenefits: String

@export var gameScene: PackedScene
