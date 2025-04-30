class_name EPQ
extends VBoxContainer

enum Type {writing, speaking, reading, maths, memory, average}
@export var barType: Type
@export var bar: Array[TextureProgressBar]

@onready var score: RichTextLabel = %Score
@onready var title: Label = %Title
@onready var single_bar: TextureProgressBar = %"Single Bar"
@onready var multi_bar: HBoxContainer = %"Multi Bar"

var progressBarColor: Color:
	set(new_value):
		progressBarColor = new_value
		for progress_bar in bar:
			progress_bar.tint_progress = progressBarColor
	
		match barType:
			Type.writing:
				single_bar.hide()
				title.text = getTitle(SaveManager.fileData.WritingEPQ)
				score.text = "[color=#999999]WRITING: [/color][b]" + str(SaveManager.fileData.WritingEPQ) + "[/b]"
				
				for progress_bar in bar:
					progress_bar.value = SaveManager.fileData.WritingEPQ
			Type.speaking:
				single_bar.hide()
				title.text = getTitle(SaveManager.fileData.SpeakingEPQ)
				score.text = "[color=#999999]SPEAKING: [/color][b]" + str(SaveManager.fileData.SpeakingEPQ) + "[/b]"
				
				for progress_bar in bar:
					progress_bar.value = SaveManager.fileData.SpeakingEPQ
			Type.reading:
				single_bar.hide()
				title.text = getTitle(SaveManager.fileData.ReadingEPQ)
				score.text = "[color=#999999]READING: [/color][b]" + str(SaveManager.fileData.ReadingEPQ) + "[/b]"
				
				for progress_bar in bar:
					progress_bar.value = SaveManager.fileData.ReadingEPQ
			Type.maths:
				single_bar.hide()
				title.text = getTitle(SaveManager.fileData.MathsEPQ)
				score.text = "[color=#999999]MATHS: [/color][b]" + str(SaveManager.fileData.MathsEPQ) + "[/b]"
				
				for progress_bar in bar:
					progress_bar.value = SaveManager.fileData.MathsEPQ
			Type.memory:
				single_bar.hide()
				title.text = getTitle(SaveManager.fileData.MemoryEPQ)
				score.text = "[color=#999999]MEMORY: [/color][b]" + str(SaveManager.fileData.MemoryEPQ) + "[/b]"
				
				for progress_bar in bar:
					progress_bar.value = SaveManager.fileData.MemoryEPQ
			Type.average:
				title.hide()
				multi_bar.hide()
				var average: float = float(SaveManager.fileData.WritingEPQ + SaveManager.fileData.SpeakingEPQ + SaveManager.fileData.ReadingEPQ + SaveManager.fileData.MathsEPQ + SaveManager.fileData.MemoryEPQ) / 5
				score.text = "[color=#999999]AVERAGE: [/color][b]" + str(average) + "[/b]"
				
				for progress_bar in bar:
					progress_bar.value = round(average)

func getTitle(value: int) -> String:
	if value >= 4750:
		return "MASTER"
	elif value >= 4250:
		return "ELITE"
	elif value >= 3750:
		return "EXPERT"
	elif value >= 2500:
		return "ADVANCED"
	elif value >= 1250:
		return "INTERMEDIATE"
	else:
		return "NOVICE"
