class_name agilityGameUI
extends CanvasLayer

@onready var word: Label = %word
@onready var option1: Button = %option1
@onready var option2: Button = %option2
@onready var question_timer: Timer = %"Question Timer"
@onready var animationPlayer: AnimationPlayer = %"gameUI animator"
@onready var score_board: HBoxContainer = %ScoreBoard
@onready var correction_panel: Control = %"Correction Panel"
@onready var sfx_right: AudioStreamPlayer = %Sfx_Right
@onready var sfx_wrong: AudioStreamPlayer = %Sfx_Wrong
@onready var instruction: Control = %Instruction
@onready var ui: Control = %UI

@export var questionSize: int = 5
@export var player: CharacterBody2D

var question: Array = Utils.loadAllResources("res://Resources/Games/agility questions/")
var questionResource: agiltyQuestion

var playerScore: int = 0:
	set(new_value):
		if new_value < 0:
			playerScore = 0
		else:
			playerScore = new_value

signal boostSpaceShip
signal endGame(score: int)

func _ready() -> void:
	questionSize = clamp(questionSize, 1, question.size())
	question.shuffle()
	
	correction_panel.hide()

func _process(_delta) -> void:
	score_board.timeleft = question_timer.time_left

func manageGameUI() -> void:
	if questionSize == 0:
		endGameUI()
	else:
		setQuestion()

func setQuestion() -> void:
	questionResource = question.pick_random()
	question.erase(questionResource)
	
	word.text = questionResource.question.to_lower()
	option1.text = ""
	option2.text = ""
	animationPlayer.play("Intro")
	await animationPlayer.animation_finished
	
	option1.text = questionResource.options[0].to_lower()
	option2.text = questionResource.options[1].to_lower()

func _on_option_1_pressed() -> void:
	if not animationPlayer.is_playing():
		option2.text = ""
		animationPlayer.play("Option1Clicked")
		await animationPlayer.animation_finished
		
		var point = questionResource.pointsDistribution[0]
		_question_done(option1, point)

func _on_option_2_pressed() -> void:
	if not animationPlayer.is_playing():
		option1.text = ""
		animationPlayer.play("Option2Clicked")
		await animationPlayer.animation_finished
		
		var point = questionResource.pointsDistribution[1]
		_question_done(option2, point)

func _question_done(buttonClicked: Button, point: int) -> void:
	questionSize -= 1
	playerScore += point
	
	if point > 0:
		sfx_right.play()
		animate(buttonClicked, "good", playerScore)
		
	else:
		sfx_wrong.play()
		animate(buttonClicked, "bad", playerScore)

func animate(button: Button, option: String, score: int) -> void:
	updateScore(score)
	match option:
		"good":
			button.modulate = Color.GREEN
			answer_animation_done()
			boostSpaceShip.emit()
		"bad":
			button.modulate = Color.RED
			correction_panel.word = questionResource.question
			match button.name:
				"option1":
					correction_panel.option1 = questionResource.options[1]
					correction_panel.option2 = questionResource.options[0]
					correction_panel.option1Meaning = questionResource.meaning[1]
					correction_panel.option2Meaning = questionResource.meaning[0]
				"option2":
					correction_panel.option1 = questionResource.options[0]
					correction_panel.option2 = questionResource.options[1]
					correction_panel.option1Meaning = questionResource.meaning[0]
					correction_panel.option2Meaning = questionResource.meaning[1]
			
			get_tree().paused = true
			correction_panel.show()

func answer_animation_done() -> void:
	await get_tree().create_timer(1).timeout
	manageGameUI()

func updateScore(score: int) -> void:
	score_board.target_score = score

func _on_correction_panel_correction_done() -> void:
	answer_animation_done()

func _on_question_timer_timeout() -> void:
	questionSize = 0
	manageGameUI()

func showInstructions() -> void:
	get_tree().paused = true
	instruction.show()
	ui.hide()

func endGameUI() -> void:
	player.queue_free()
	question_timer.stop()
	endGame.emit(playerScore)

func _on_death_zone_player_outof_bound() -> void:
	endGameUI()
