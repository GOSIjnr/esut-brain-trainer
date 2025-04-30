class_name UserData
extends Resource

@export var UserName: String = "User"
@export var isTutorialDone: bool = false

@export var Writing: bool = false
@export var Speaking: bool = false
@export var Reading: bool = false
@export var Maths: bool = false
@export var Memory: bool = false

@export var starting_WritingEPQ: int = 0
@export var starting_SpeakingEPQ: int = 0
@export var starting_ReadingEPQ: int = 0
@export var starting_MathsEPQ: int = 0
@export var starting_MemoryEPQ: int = 0

@export var WritingEPQ: int = 0:
	set(new_value):
		WritingEPQ = clamp(new_value, 10, 5000)

@export var SpeakingEPQ: int = 0:
	set(new_value):
		SpeakingEPQ = clamp(new_value, 10, 5000)

@export var ReadingEPQ: int = 0:
	set(new_value):
		ReadingEPQ = clamp(new_value, 10, 5000)

@export var MathsEPQ: int = 0:
	set(new_value):
		MathsEPQ = clamp(new_value, 10, 5000)

@export var MemoryEPQ: int = 0:
	set(new_value):
		MemoryEPQ = clamp(new_value, 10, 5000)

@export var WritingDaily: Dictionary = {}
@export var SpeakingDaily: Dictionary = {}
@export var ReadingDaily: Dictionary = {}
@export var MathsDaily: Dictionary = {}
@export var MemoryDaily: Dictionary = {}
@export var AverageDaily: Dictionary = {}

@export var Workouts: int = 0

@export var HighScores: Dictionary = {}
