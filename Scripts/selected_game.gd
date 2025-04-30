class_name selectedGame
extends Control

@onready var gameName: Label = %game_name
@onready var gameType: Label = %game_type
@onready var gameIcon: TextureRect = %game_icon
@onready var gameBackground: TextureRect = %Background
@onready var hex: TextureRect = %hex
@onready var benefits: RichTextLabel = %Benefits
@onready var startButton: Button = %startGame
@onready var highScore: Label = %HighScore
@onready var help: Panel = %Help
@onready var helpText: RichTextLabel = %HelpText

var selected :Games:
	set(resource):
		selected = resource
		gameName.text = resource.gameName
		gameIcon.texture = resource.gameIcon
		gameBackground.texture = resource.gameFullBackground
		hex.self_modulate = resource.gameColor
		highScore.text = str(SaveManager.fileData.HighScores[resource.gameName.to_lower()])
		updateStartButton(resource.gameColor)
		updateText(benefits, "[color=#cccccc][b]BENEFITS:[/b][/color]", resource.gameBenefits, "")
		updateText(helpText, "[b]INSTRUCTIONS[/b]", resource.howToPlay, "[center][b]Tap to close[/b][/center]")
	
		if resource.gameScene == null:
			startButton.disabled = true
	
		var gameRef = ["Writing", "Speaking", "Reading", "Maths", "Memory"]
		gameType.text = gameRef[resource.gameType]

func updateStartButton(color :Color) -> void:
	var darken := 0.15
	
	var styleBoxflat = StyleBoxFlat.new()
	styleBoxflat.bg_color = color
	styleBoxflat.corner_detail = 12
	styleBoxflat.border_width_bottom = 15
	styleBoxflat.border_color = color.darkened(darken)
	styleBoxflat.set_corner_radius_all(20)
	
	var styleBoxclicked = StyleBoxFlat.new()
	styleBoxclicked.bg_color = color.darkened(darken)
	styleBoxclicked.corner_detail = 12
	styleBoxclicked.set_corner_radius_all(20)
	
	startButton.add_theme_stylebox_override("hover", styleBoxflat)
	startButton.add_theme_stylebox_override("normal", styleBoxflat)
	startButton.add_theme_stylebox_override("pressed", styleBoxclicked)

func updateText(text :RichTextLabel, suffix :String, textToAdd :String, prefix :String) -> void:
	text.clear()
	text.append_text(suffix)
	text.add_text("\n\n")
	text.append_text(textToAdd)
	
	if not prefix.is_empty():
		text.add_text("\n\n")
		text.append_text(prefix)

func _on_close_button_gui_input(event: InputEvent) -> void:
	if event is InputEventScreenTouch and event.is_released():
		var rect = Rect2(Vector2(0, 0), Vector2(100, 100))
		
		if rect.has_point(event.position):
			Global.selectedGameResource = null
			self.queue_free()

func _on_help_button_gui_input(event) -> void:
	if event is InputEventScreenTouch and event.is_released():
		var rect = Rect2(Vector2(0, 0), Vector2(100, 100))
		
		if rect.has_point(event.position):
			help.visible = true
			help.modulate = Color(1, 1, 1, 0)
		
			var tween = create_tween()
			tween.tween_property(help, "modulate", Color(1, 1, 1, 1), 0.05)

func _on_start_game_pressed() -> void:
	get_tree().change_scene_to_packed(selected.gameScene)

func _on_help_gui_input(event: InputEvent) -> void:
	if event is InputEventScreenTouch and event.is_released():
		help.hide()

func _notification(what) -> void:
	match what:
		NOTIFICATION_WM_GO_BACK_REQUEST:
			go_back_request()

func go_back_request() -> void:
	if help.visible == true:
		help.hide()
	else:
		self.queue_free()
