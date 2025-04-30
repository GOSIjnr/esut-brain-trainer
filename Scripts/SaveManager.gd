class_name saveManager
extends Node

var gamefilePath: String = "user://UserData.tres"
var fileData: UserData

var newScores: Dictionary = {
	"agility": 0,
	"average": 0,
	"aviodance": 0,
	"collaspe": 0,
	"memory": 0,
	"pronunciation": 0,
	"sound match": 0,
	"syntax": 0,
	"tipping": 0,
	"word part": 0,
}

func loadData():
	fileData = load(gamefilePath)

func saveData():
	ResourceSaver.save(fileData, gamefilePath)

func newData():
	fileData = UserData.new()
	fileData.HighScores = Utils.updateDictionary(fileData.HighScores, newScores)
	ResourceSaver.save(fileData, gamefilePath)
