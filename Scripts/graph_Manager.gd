class_name graphManager
extends Control

@export var lineWidth: int = 3

var min_value: int = 0
var max_value: int = 5000
var dataRange: int
var dataColor: Color

# Nodes for the graph
var line: Line2D
var fill_polygon: Polygon2D
var borderLine: Line2D

# Padding for labels
var label_padding: int = 20

var data_dict: Dictionary:
	set(new_value):
		data_dict = new_value
		
		for child in self.get_children():
			child.queue_free()
		
		line = Line2D.new()
		self.add_child(line)
		fill_polygon = Polygon2D.new()
		self.add_child(fill_polygon)
		borderLine = Line2D.new()
		self.add_child(borderLine)
		
		line.width = lineWidth
		line.default_color = dataColor
		fill_polygon.color = Color(dataColor.r, dataColor.g, dataColor.b, 0.5)
		borderLine.width = 2
		borderLine.default_color = Color(1, 1, 1)
		
		call_deferred("update_graph")

func update_graph() -> void:
	line.clear_points()
	fill_polygon.polygon = []

	var graph_width = size.x - 100
	var graph_height = size.y
	
	add_y_axis_labels(graph_width, graph_height)
	plot_data_for_range(dataRange, graph_width, graph_height)
	draw_border()

func draw_border() -> void:
	var top_left = Vector2.ZERO
	var top_right = Vector2(size.x, 0)
	var bottom_right = size
	var bottom_left = Vector2(0, size.y)
	
	borderLine.points = [top_left, top_right, bottom_right, bottom_left, top_left]

func plot_data_for_range(days: int, graph_width: int, graph_height: int) -> void:
	var current_date_str: String = Time.get_datetime_string_from_unix_time(int(Time.get_unix_time_from_system())).substr(0, 10)
	var current_timestamp: int = date_to_timestamp(current_date_str)
	
	var filtered_data: Dictionary = {}
	
	for date_key in data_dict.keys():
		var date_timestamp = date_to_timestamp(date_key)
		if date_timestamp >= current_timestamp - (days * 86400):
			filtered_data[date_key] = data_dict[date_key]
	
	plot_filtered_data(filtered_data, graph_width, graph_height)

func plot_filtered_data(filtered_data: Dictionary, graph_width: int, graph_height: int) -> void:
	line.clear_points()
	var dates = filtered_data.keys()
	dates.sort()
	
	var spacing = graph_width / float(dates.size() - 1)
	var points = []
	
	for i in range(dates.size()):
		var date = dates[i]
		var value = filtered_data[date]
		var normalized_value = (value - min_value) / float(max_value - min_value) * graph_height
		var plot = Vector2(i * spacing, graph_height - normalized_value)
		points.append(plot)
	
	var smooth_points = []
	if points.size() > 2:
		smooth_points = points
	else:
		smooth_points = points
	
	for p in smooth_points:
		line.add_point(p)
	
	var polygon_points = points.duplicate()
	polygon_points.insert(0, Vector2(0, graph_height))
	polygon_points.append(Vector2(graph_width, graph_height))
	fill_polygon.polygon = polygon_points

func date_to_timestamp(date_str: String) -> int:
	return Time.get_unix_time_from_datetime_string(date_str + "T00:00:00")

func add_y_axis_labels(graph_width: int, graph_height: int) -> void:
	var step_value = float(max_value - min_value) / 5
	
	for i in range(5):
		var value = min_value + i * step_value
		var normalized_value = (value - min_value) / float(max_value - min_value) * graph_height
		var y_position = graph_height - normalized_value
		
		if value != 0:
			var label = Label.new()
			label.text = str(value)
			label.custom_minimum_size = Vector2(100, 20)
			label.position = Vector2(graph_width + label_padding, y_position - label.custom_minimum_size.y / 2)
			label.theme = preload("res://Themes/LightMode.tres")
			label.theme_type_variation = "Label_Mid"
			label.add_theme_color_override("font_color", Color.WHITE)
			self.add_child(label)
		
			var h_line = Line2D.new()
			h_line.width = 1
			h_line.default_color = Color(1, 1, 1, 0.5)
			h_line.add_point(Vector2(0, y_position))
			h_line.add_point(Vector2(graph_width, y_position))
			add_child(h_line)
