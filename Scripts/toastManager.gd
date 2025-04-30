extends Control

const PADDING: Vector2 = Vector2(100, 60)
const ANIM: float = 0.2

@onready var panel: Panel = %Panel
@onready var toastMessage: Label = %Label

func showMessage(message: String, duration: float) -> void:
	if Global.isToastActive == false:
		Global.isToastActive = true
		toastMessage.text = message
		toastMessage.update_minimum_size()
		panel.custom_minimum_size = toastMessage.get_minimum_size() + PADDING
		animate(0, 1, ANIM)
		await get_tree().create_timer(duration).timeout
		hideMessage()

func hideMessage():
	await animate(1, 0, ANIM)
	await get_tree().create_timer(ANIM).timeout
	Global.isToastActive = false
	self.queue_free()

func animate(currentValue: float, newValue: float, duration: float):
	self.modulate = Color(1, 1, 1, currentValue)
	
	var tween = create_tween()
	tween.tween_property(self, "modulate", Color(1, 1, 1, newValue), duration)
