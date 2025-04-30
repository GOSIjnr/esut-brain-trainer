class_name tutorialGame
extends Resource

@export_multiline var question: String
enum pointType {writing, speaking, reading, maths, memory}
@export var QuestionType: pointType

@export var options: Array[String]
@export var pointsDistribution: Array[int]
