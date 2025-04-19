using Godot;

namespace GOSIjnr;

public partial class WelcomePage5 : ContentPage
{
	private Button _nextButton;

	public override void _Ready()
	{
		_nextButton = GetNode<Button>("%Button");
		_nextButton.Pressed += NextPage;
	}

	public override void _ExitTree()
	{
		_nextButton.Pressed -= NextPage;
	}

	private void NextPage()
	{
		EmitSignalNextPageButtonClick();
	}
}

