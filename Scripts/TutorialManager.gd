class_name tutorialManager
extends Control

var CurrentPage: Control
var NextPage: Control
var currentPosition: int = 0
var scrollPositions: Array[int]

@onready var scrollView: HBoxContainer = $"Page 2/Content"
@onready var previousButton: Button = $"Page 2/MarginContainer/UI/HBoxContainer/Button1"
@onready var nextButton: Button = $"Page 2/MarginContainer/UI/HBoxContainer/Button2"

func _ready() -> void:
	SaveManager.loadData()
	get_tree().quit_on_go_back = false
	
	var scrollSection = scrollView.size.x / 3
	scrollPositions = [0, -scrollSection, -scrollSection * 2]

func _process(_delta) -> void:
	page4UpdateUI()

# page1
func _on_page1NextButton_pressed() -> void:
	CurrentPage = $"Page 1"
	NextPage = $"Page 2"
	CurrentPage.hide()
	NextPage.show()

# page2
func _on_buttonPrevious_pressed() -> void:
	currentPosition -= 1
	
	if currentPosition < 0:
		currentPosition = 0
	
	updateUI(currentPosition)

func _on_buttonNext_pressed() -> void:
	currentPosition += 1
	
	if currentPosition > 2:
		currentPosition = 2
		CurrentPage = $"Page 2"
		NextPage = $"Page 3"
		CurrentPage.hide()
		NextPage.show()
	else:
		updateUI(currentPosition)

func updateUI(new_position) -> void:
	var tween = create_tween()
	tween.tween_property(scrollView, "position", Vector2(scrollPositions[new_position], 0), .15)
	
	match new_position:
		0:
			previousButton.disabled = true
		1:
			previousButton.disabled = false
			nextButton.text = "Next"
		2:
			nextButton.text = "Continue"

# page3
func _on_Page3NextButton_pressed() -> void:
	CurrentPage = $"Page 3"
	NextPage = $"Page 4"
	CurrentPage.hide()
	NextPage.show()

# page4
func _on_Page4NextButton_pressed() -> void:
	CurrentPage = $"Page 4"
	NextPage = $"Page 5"
	CurrentPage.hide()
	NextPage.show()
	savePersonalization()

func page4UpdateUI() -> void:
	if checkCheckedCheckboxes() and $"Page 4".visible == true:
		$"Page 4/MarginContainer/Button".disabled = false
	else:
		$"Page 4/MarginContainer/Button".disabled = true

func checkCheckedCheckboxes() -> bool:
	var checkBoxContainer = $"Page 4/MarginContainer/VBoxContainer/GridContainer"
	var num_checkboxes = checkBoxContainer.get_child_count()
	var any_checked = false
	
	for i in range(num_checkboxes):
		var checkbox = checkBoxContainer.get_child(i)
		if checkbox is Button and checkbox.button_pressed:
			any_checked = true
			break
	
	return any_checked

func savePersonalization() -> void:
	SaveManager.fileData.Writing = %WritingCB.button_pressed
	SaveManager.fileData.Speaking = %SpeakingCB.button_pressed
	SaveManager.fileData.Reading = %ReadingCB.button_pressed
	SaveManager.fileData.Maths = %MathsCB.button_pressed
	SaveManager.fileData.Memory = %MemoryCB.button_pressed
	
	SaveManager.saveData()

# page5
func _on_Page5NextButton_pressed() -> void:
	get_tree().change_scene_to_packed(SceneLoader.get_resource("tutorial_game"))

func _notification(what) -> void:
	match what:
		NOTIFICATION_WM_GO_BACK_REQUEST:
			go_back_request()

func go_back_request() -> void:
	if $"Page 1".visible == true:
		get_tree().quit()
		return
	
	if $"Page 2".visible == true:
		if currentPosition == 0:
			$"Page 2".hide()
			$"Page 1".show()
		else :
			_on_buttonPrevious_pressed()
		return
	
	if $"Page 3".visible == true:
		$"Page 3".hide()
		$"Page 2".show()
		return
	
	if $"Page 4".visible == true:
		$"Page 4".hide()
		$"Page 3".show()
		return
	
	if $"Page 5".visible == true:
		$"Page 5".hide()
		$"Page 4".show()
		return
