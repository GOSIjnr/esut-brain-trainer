using Godot;

public partial class WelcomePage1 : ContentPage
{
	private Button _nextButton;

	public override void _EnterTree()
	{
		_nextButton = GetNode<Button>("%Button");
		_nextButton.Pressed += EmitSignalOnNextPageRequested;
	}

	public override void _ExitTree()
	{
		if (_nextButton != null) return;

		_nextButton.Pressed -= EmitSignalOnNextPageRequested;
	}
}
