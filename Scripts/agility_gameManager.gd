class_name agilityGameManger
extends Node

@onready var gameUI: agilityGameUI = %GameUI
@onready var pause_ui: CanvasLayer = %PauseUI
@onready var end_game: agiltyEndGame = %EndGame

func _ready() -> void:
	SaveManager.loadData()
	get_tree().quit_on_go_back = false
	pause_ui.hide()
	end_game.hide()
	
	var _selectedGame: Games = Global.selectedGameResource
	var _score: Dictionary = SaveManager.fileData.HighScores
	
	if _selectedGame != null and _score[_selectedGame.gameName.to_lower()] == 0:
		gameUI.showInstructions()
	else:
		gameUI.manageGameUI()

func _notification(what) -> void:
	match what:
		NOTIFICATION_WM_GO_BACK_REQUEST:
			call_deferred("_on_score_board_pause_game")

func _on_game_ui_end_game(score: int) -> void:
	get_tree().paused = true
	gameUI.hide()
	end_game.score = score
	end_game.show()

func _on_score_board_pause_game() -> void:
	get_tree().paused = true
	pause_ui.show()

func _on_instruction_instruction_closed() -> void:
	gameUI.manageGameUI()

func _on_game_ui_start_game() -> void:
	gameUI.manageGameUI()

func _on_death_zone_player_outof_bound() -> void:
	_on_game_ui_end_game(gameUI.playerScore)
