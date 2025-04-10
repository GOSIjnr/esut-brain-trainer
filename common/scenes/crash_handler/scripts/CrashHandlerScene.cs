using Godot;

public partial class CrashHandlerScene : CanvasLayer
{
	private Button _closeButton;

	public override void _EnterTree()
	{
		_closeButton = GetNode<Button>("%Quit");
		_closeButton.Pressed += OnCloseButtonPressed;
	}

	public override void _ExitTree()
	{
		_closeButton.Pressed -= OnCloseButtonPressed;
	}

	public void OnCloseButtonPressed()
	{
		GetTree().Quit();
	}
}
