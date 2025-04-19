using Godot;

public partial class WelcomePage3 : ContentPage
{
	private Button _nextButton;

	public override void _Ready()
	{
		_nextButton = GetNodeOrNull<Button>("%Button");

		if (_nextButton == null) return;

		_nextButton.Pressed += EmitSignalOnNextPageRequested;
	}

	public override void _ExitTree()
	{
		if (_nextButton != null) return;

		_nextButton.Pressed -= EmitSignalOnNextPageRequested;
	}
}
