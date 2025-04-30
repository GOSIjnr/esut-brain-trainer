class_name tutorialGameManager
extends Control

@export var timerNormal: Color
@export var timerWarning: Color
@export var waitTime: float = 2.0

var questionsPath: String = "res://Resources/TutorialGame/"
var questions: Array = []
var questionIndex: int = 0

var writingScore: int = 0
var speakingScore: int = 0
var readingScore: int = 0
var mathsScore: int = 0
var memoryScore: int = 0

var elaspedTime: float = 0.0

@onready var question: RichTextLabel = %Question
@onready var progress: TextureProgressBar = %Progress
@onready var option_1: Button = %Option1
@onready var option_2: Button = %Option2
@onready var option_3: Button = %Option3
@onready var option_4: Button = %Option4
@onready var option_5: Button = %Option5
@onready var question_timer: Timer = %QuestionTimer
@onready var timer_countdown: Label = %TimerCountdown
@onready var sfx_right: AudioStreamPlayer = %Sfx_Right
@onready var sfx_wrong: AudioStreamPlayer = %Sfx_Wrong
@onready var sfx_tick: AudioStreamPlayer = %Sfx_Tick

func _ready() -> void:
	SaveManager.loadData()
	get_tree().quit_on_go_back = false
	var setter = get_tree().get_nodes_in_group("questions")
	
	for button in setter:
		if button is Button:
			button.pressed.connect(optionClicked.bind(button	))
	
	questions = Utils.loadAllResources(questionsPath)
	questions.shuffle()
	progress.max_value = questions.size()
	displayQuestion(questionIndex)

func _process(delta) -> void:
	var time = round(question_timer.time_left)
	var formattedTime = "%02d" % time
	timer_countdown.text = ":" + formattedTime
	
	if time <= 11:
		if time <= 10:
			timer_countdown.self_modulate = timerWarning
		
		elaspedTime += delta
		if elaspedTime >= 1.0:
			if not sfx_tick.playing and not question_timer.time_left == 0:
				elaspedTime = 0
				sfx_tick.play()
	else:
		timer_countdown.self_modulate = timerNormal

func resetAll() -> void:
	var setters = get_tree().get_nodes_in_group("questions")
	
	for text in setters:
		if text is Button:
			text.hide()
			text.disabled = false
			text.theme_type_variation = ""
		else:
			text.hide()

func displayQuestion(index :int) -> void:
	if index > questions.size() - 1:
		SaveManager.fileData.WritingEPQ = writingScore
		SaveManager.fileData.SpeakingEPQ = speakingScore
		SaveManager.fileData.ReadingEPQ = readingScore
		SaveManager.fileData.MathsEPQ = mathsScore
		SaveManager.fileData.MemoryEPQ = memoryScore
		
		SaveManager.fileData.starting_WritingEPQ = SaveManager.fileData.WritingEPQ
		SaveManager.fileData.starting_SpeakingEPQ = SaveManager.fileData.SpeakingEPQ
		SaveManager.fileData.starting_ReadingEPQ = SaveManager.fileData.ReadingEPQ
		SaveManager.fileData.starting_MathsEPQ = SaveManager.fileData.MathsEPQ
		SaveManager.fileData.starting_MemoryEPQ = SaveManager.fileData.MemoryEPQ
		
		SaveManager.saveData()
		get_tree().change_scene_to_packed(SceneLoader.get_resource("tutorial_end"))
	else:
		resetAll()
		progress.value = index + 1
		question.show()
		var _resource: tutorialGame = questions[index]
		question.text = _resource.question
		displayOptions(_resource, _resource.options.size())
		question_timer.start()

func displayOptions(selectedRes :tutorialGame, lengthSize :int) -> void:
	option_5.show()
	
	match lengthSize:
		2:
			option_1.show()
			option_2.show()
			option_1.text = selectedRes.options[0]
			option_2.text = selectedRes.options[1]
		4:
			option_1.show()
			option_2.show()
			option_3.show()
			option_4.show()
			option_1.text = selectedRes.options[0]
			option_2.text = selectedRes.options[1]
			option_3.text = selectedRes.options[2]
			option_4.text = selectedRes.options[3]

func disableOptions() -> void:
	var setters = get_tree().get_nodes_in_group("questions")
	
	for text in setters:
		if text is Button:
			text.disabled = true

func getTheRightOption(_resource: Array[int]) -> void:
	var rightOption: int = 0
	
	for number in range(_resource.size()):
		if _resource[number] > 0:
			rightOption = number + 1
			break
	
	match rightOption:
		1:
			option_1.theme_type_variation = "Button_Right"
		2:
			option_2.theme_type_variation = "Button_Right"
		3:
			option_3.theme_type_variation = "Button_Right"
		4:
			option_4.theme_type_variation = "Button_Right"

func scorePlayer(_resource :tutorialGame, _points :int) -> void:
	match _resource.QuestionType:
		0:
			writingScore += _points
		1:
			speakingScore += _points
		2:
			readingScore += _points
		3:
			mathsScore += _points
		4:
			memoryScore += _points

func _on_question_timer_timeout() -> void:
	option_5.emit_signal("pressed")

func _notification(what) -> void:
	match what:
		NOTIFICATION_WM_GO_BACK_REQUEST:
			Utils.showToast(get_tree().root, "Can't go back at this stage", 1.5)

func optionClicked(_button: BaseButton) -> void:
	disableOptions()
	var _clicked: int = 0
	var _resource: tutorialGame = questions[questionIndex]
	var _questionSkipped: bool = false
	
	match _button.name:
		"Option1":
			_clicked = 0
		"Option2":
			_clicked = 1
		"Option3":
			_clicked = 2
		"Option4":
			_clicked = 3
		"Option5":
			_questionSkipped = true
	
	if _questionSkipped:
		disableOptions()
		sfx_wrong.play()
		
		if not _resource.QuestionType == _resource.pointType.speaking or _resource.QuestionType == _resource.pointType.memory:
			getTheRightOption(_resource.pointsDistribution)
	else:
		if _resource.pointsDistribution[_clicked] > 0:
			sfx_right.play()
			_button.theme_type_variation = "Button_Right"
			scorePlayer(_resource, _resource.pointsDistribution[_clicked])
		else:
			sfx_wrong.play()
			_button.theme_type_variation = "Button_Wrong"
			getTheRightOption(_resource.pointsDistribution)
	
	waitTimer()

func waitTimer() -> void:
	await get_tree().create_timer(waitTime).timeout
	questionIndex += 1
	displayQuestion(questionIndex)
