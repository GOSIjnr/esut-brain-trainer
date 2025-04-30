class_name redirect
extends Panel

var File = FileAccess

func _ready() -> void:
	get_tree().quit_on_go_back = false
	
	var check = userDataCheck()
	if check:
		SaveManager.loadData()
		SaveManager.fileData.HighScores = Utils.updateDictionary(SaveManager.fileData.HighScores, SaveManager.newScores)
		SaveManager.saveData()
	else:
		SaveManager.newData()
	
	SaveManager.loadData()
	if SaveManager.fileData.isTutorialDone:
		call_deferred("load_main_menu")
	else:
		call_deferred("load_tutorial")

func userDataCheck() -> bool:
	if File.file_exists(SaveManager.gamefilePath):
		return true
	else:
		return false

func load_main_menu() -> void:
	get_tree().change_scene_to_packed(SceneLoader.get_resource("main_menu"))

func load_tutorial() -> void:
	get_tree().change_scene_to_packed(SceneLoader.get_resource("tutorial_start"))
