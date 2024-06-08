extends Button

func _on_pressed():
	get_tree().set_pause(false)
	get_tree().reload_current_scene()
