class_name agiltyEndGame
extends CanvasLayer

@onready var iconBackground: TextureRect = %"Icon Background"
@onready var gameIcon: TextureRect = %"Game Icon"
@onready var scoreText: Label = %Score
@onready var remarks: Label = %Remarks
@onready var proficiency: Label = %Proficiency
@onready var continueText: Label = %Continue

var score: int = 0
var gameResource: Games = Global.selectedGameResource

func _ready() -> void:
	self.visibility_changed.connect(updateUI)

func updateUI() -> void:
	if not gameResource == null and self.visible == true:
		iconBackground.self_modulate = gameResource.gameColor
		gameIcon.texture = gameResource.gameIcon
		scoreText.text = str(score)
		scoreText.self_modulate = gameResource.gameColor
		remarks.text = getRemark()
		proficiency.text = getProficiency()

func _on_panel_gui_input(event: InputEvent) -> void:
	if event is InputEventScreenTouch and event.is_released():
		get_tree().paused = false
		get_tree().change_scene_to_packed(SceneLoader.get_resource("main_menu"))

func getRemark() -> String:
	var remark: String = ""
	
	if score >= 6000:
		remark += "excellent!"
	elif score >= 4000:
		remark += "goodjob."
	elif score >= 2000:
		remark += "pass."
	else:
		remark += "fail!"
	
	if score > SaveManager.fileData.HighScores[gameResource.gameName.to_lower()]:
		remark += " highscore!"
		SaveManager.fileData.HighScores[gameResource.gameName.to_lower()] = score
		ResourceSaver.save(SaveManager.fileData, SaveManager.gamefilePath)
	
	return remark

func getProficiency() -> String:
	var text: String = ""
	
	match gameResource.gameType:
		gameResource.Type.writing:
			text += str(abs(proficiencyScore())) + " Writing"
			SaveManager.fileData.WritingEPQ += proficiencyScore()
		gameResource.Type.speaking:
			text += str(abs(proficiencyScore())) + " Speaking"
			SaveManager.fileData.SpeakingEPQ += proficiencyScore()
		gameResource.Type.reading:
			text += str(abs(proficiencyScore())) + " Reading"
			SaveManager.fileData.ReadingEPQ += proficiencyScore()
		gameResource.Type.maths:
			text += str(abs(proficiencyScore())) + " Maths"
			SaveManager.fileData.MathsEPQ += proficiencyScore()
		gameResource.Type.memory:
			text += str(abs(proficiencyScore())) + " Memory"
			SaveManager.fileData.MemoryEPQ += proficiencyScore()
	
	if proficiencyScore() > 0:
		text += " Proficiency Gained"
	else:
		text += " Proficiency Deducted"
	
	ResourceSaver.save(SaveManager.fileData, SaveManager.gamefilePath)
	
	return text

func proficiencyScore() -> int:
	if score >= 6000:
		return 90
	elif score >= 4000:
		return 45
	elif score >= 2000:
		return 15
	else:
		return -45
