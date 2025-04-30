class_name dailyManager
extends Node

func _ready() -> void:
	SaveManager.loadData()
	
	var average = SaveManager.fileData.WritingEPQ + SaveManager.fileData.SpeakingEPQ + SaveManager.fileData.ReadingEPQ + SaveManager.fileData.MathsEPQ + SaveManager.fileData.MemoryEPQ
	average /= 5
	
	addData(SaveManager.fileData.AverageDaily, average)
	addData(SaveManager.fileData.WritingDaily, SaveManager.fileData.WritingEPQ)
	addData(SaveManager.fileData.SpeakingDaily, SaveManager.fileData.SpeakingEPQ)
	addData(SaveManager.fileData.ReadingDaily, SaveManager.fileData.ReadingEPQ)
	addData(SaveManager.fileData.MathsDaily, SaveManager.fileData.MathsEPQ)
	addData(SaveManager.fileData.MemoryDaily, SaveManager.fileData.MemoryEPQ)
	
	SaveManager.saveData()

func date_to_timestamp(date_str: String) -> int:
	return Time.get_unix_time_from_datetime_string(date_str + "T00:00:00")

func timestamp_to_date(timestamp: int) -> String:
	return Time.get_datetime_string_from_unix_time(timestamp).substr(0, 10)

func addData(container: Dictionary, data: int) -> void:
	var current_date_str: String = Time.get_datetime_string_from_unix_time(int(Time.get_unix_time_from_system())).substr(0, 10)
	var current_timestamp: int = date_to_timestamp(current_date_str)
	
	if container.is_empty():
		container[current_date_str] = data
		return
	
	var dates: Array = container.keys()
	dates.sort()
	
	var last_key_date_str: String = dates.back()
	var last_key_timestamp: int = date_to_timestamp(last_key_date_str)
	
	if current_timestamp < date_to_timestamp(dates[0]):
		return
	
	if current_timestamp > last_key_timestamp:
		var previous_data = container[dates.back()]
		for date in range(last_key_timestamp + 86400, current_timestamp + 86400, 86400):
			container[timestamp_to_date(date)] = previous_data
		
		container[current_date_str] = data
		return
	
	if  current_timestamp == last_key_timestamp:
		container[current_date_str] = data
