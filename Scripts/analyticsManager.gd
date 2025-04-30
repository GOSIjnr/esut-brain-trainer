class_name analyticsChart
extends Control

@onready var background: Panel = %Background
@onready var title: Label = %Title
@onready var info: Label = %Info
@onready var graph: Control = %Graph
@onready var option_button: OptionButton = %OptionButton
@onready var progress_high: TextureProgressBar = %progressHigh
@onready var progress_starting: TextureProgressBar = %progressStarting
@onready var progress_low: TextureProgressBar = %progressLow
@onready var growth: Label = %Growth
@onready var starting_pq: Label = %StartingPQ
@onready var current_pq: Label = %CurrentPQ
@onready var proficiency: Label = %Proficiency

var PQColor: Color
var selectedIndex: int = 0

var type: String:
	set(new_value):
		if SaveManager.fileData == null: return 
		type = new_value
		title.text = new_value
		
		option_button.select(selectedIndex)
		_on_option_button_item_selected(selectedIndex)
		matchColor(new_value)
		updateText()

func _on_option_button_child_entered_tree(node: Node) -> void:
	if node is PopupMenu:
		node.transparent_bg = true

func _on_option_button_item_selected(index: int) -> void:
	var text: String = option_button.get_item_text(index)
	info.text = "Data for your " + title.text.to_lower() + " PQ for the last " + text
	selectedIndex = index
	plotGraph(int(text))

func matchColor(typeName: String) -> void:
	match typeName:
		"Writing":
			PQColor = Global.PQColor[0]
		"Speaking":
			PQColor = Global.PQColor[1]
		"Reading":
			PQColor = Global.PQColor[2]
		"Maths":
			PQColor = Global.PQColor[3]
		"Memory":
			PQColor = Global.PQColor[4]
		"Average":
			PQColor = Global.PQColor[5]
	
	background.self_modulate = Color(PQColor.r, PQColor.g, PQColor.b, 0.5)
	progress_high.tint_progress = PQColor
	progress_starting.tint_progress = PQColor.darkened(0.2)
	progress_low.tint_progress = PQColor.darkened(0.4)

func plotGraph(days: int) -> void:
	var _ref = graph as graphManager
	_ref.dataRange = days
	
	match type:
		"Writing":
			_ref.dataColor = Global.PQColor[0]
			_ref.data_dict = SaveManager.fileData.WritingDaily
		"Speaking":
			_ref.dataColor = Global.PQColor[1]
			_ref.data_dict = SaveManager.fileData.SpeakingDaily
		"Reading":
			_ref.dataColor = Global.PQColor[2]
			_ref.data_dict = SaveManager.fileData.ReadingDaily
		"Maths":
			_ref.dataColor = Global.PQColor[3]
			_ref.data_dict = SaveManager.fileData.MathsDaily
		"Memory":
			_ref.dataColor = Global.PQColor[4]
			_ref.data_dict = SaveManager.fileData.MemoryDaily
		"Average":
			_ref.dataColor = Global.PQColor[5]
			_ref.data_dict = SaveManager.fileData.AverageDaily

func updateText() -> void:
	match type:
		"Writing":
			proficiency.text = getTitle(SaveManager.fileData.WritingEPQ)
			current_pq.text = str(SaveManager.fileData.WritingEPQ)
			starting_pq.text = str(SaveManager.fileData.starting_WritingEPQ)
		"Speaking":
			proficiency.text = getTitle(SaveManager.fileData.SpeakingEPQ)
			current_pq.text = str(SaveManager.fileData.SpeakingEPQ)
			starting_pq.text = str(SaveManager.fileData.starting_SpeakingEPQ)
		"Reading":
			proficiency.text = getTitle(SaveManager.fileData.ReadingEPQ)
			current_pq.text = str(SaveManager.fileData.ReadingEPQ)
			starting_pq.text = str(SaveManager.fileData.starting_ReadingEPQ)
		"Maths":
			proficiency.text = getTitle(SaveManager.fileData.MathsEPQ)
			current_pq.text = str(SaveManager.fileData.MathsEPQ)
			starting_pq.text = str(SaveManager.fileData.starting_MathsEPQ)
		"Memory":
			proficiency.text = getTitle(SaveManager.fileData.MemoryEPQ)
			current_pq.text = str(SaveManager.fileData.MemoryEPQ)
			starting_pq.text = str(SaveManager.fileData.starting_MemoryEPQ)
		"Average":
			proficiency.text = getTitle(SaveManager.fileData.SpeakingEPQ)
			current_pq.text = str(SaveManager.fileData.SpeakingEPQ)
			starting_pq.text = str(SaveManager.fileData.starting_SpeakingEPQ)
	
	updateBar(int(current_pq.text), int(starting_pq.text))
	growth.text = updateGrowth(int(current_pq.text), int(starting_pq.text))


func getTitle(value: int) -> String:
	if value >= 4750:
		return "Master"
	elif value >= 4250:
		return "Elite"
	elif value >= 3750:
		return "Expert"
	elif value >= 2500:
		return "Advanced"
	elif value >= 1250:
		return "Intermediate"
	else:
		return "Novice"

func updateBar(current: int, starting: int) -> void:
	progress_starting.value = starting
	
	if current >= starting:
		progress_high.value = current
		progress_low.value = 0
	else:
		progress_high.value = 0
		progress_low.value = current

func updateGrowth(current: float, starting: float) -> String:
	var _decimalMultiplex = 10
	
	if current > starting:
		var _difference = current - starting
		var _number = (_difference / starting) * 100
		var _rounded = round(_number * _decimalMultiplex) / _decimalMultiplex
		var _percentage = "+" + str(_rounded) + "%"
		growth.self_modulate = Color.SEA_GREEN
		return _percentage
	elif current < starting:
		var _difference = starting - current
		var _number = (_difference / starting) * 100
		var _rounded = round(_number * _decimalMultiplex) / _decimalMultiplex
		var _percentage = "-" + str(_rounded) + "%"
		growth.self_modulate = Color.INDIAN_RED
		return _percentage
	else:
		growth.self_modulate = Color.WHITE
		return "0%"
