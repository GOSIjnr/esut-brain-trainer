using Godot;

public partial class IntroController : MarginContainer
{
	private Button _previousButton;
	private Button _nextButton;

	[Signal] public delegate void PreviousButtonClickedEventHandler();
	[Signal] public delegate void NextButtonClickedEventHandler();

	public override void _EnterTree()
	{
		_previousButton = GetNode<Button>("%Button1");
		_nextButton = GetNode<Button>("%Button2");

		_previousButton.Pressed += EmitSignalPreviousButtonClicked;
		_nextButton.Pressed += EmitSignalNextButtonClicked;
	}

	public override void _ExitTree()
	{
		_previousButton.Pressed -= EmitSignalPreviousButtonClicked;
		_nextButton.Pressed -= EmitSignalNextButtonClicked;
	}

	public void UpdateUIForCurrentIndex(int currentIndex, int totalCount)
	{
		_previousButton.Disabled = false;
		_nextButton.Disabled = false;
		_nextButton.Text = "BUTTON_NEXT";

		if (currentIndex <= 0)
		{
			_previousButton.Disabled = true;
		}

		if (currentIndex >= totalCount - 1)
		{
			_nextButton.Text = "BUTTON_CONTINUE";
		}
	}

	public void DisableControl(bool isDisable)
	{
		if (isDisable)
		{
			_previousButton.MouseFilter = MouseFilterEnum.Ignore;
			_nextButton.MouseFilter = MouseFilterEnum.Ignore;
		}
		else
		{
			_previousButton.MouseFilter = MouseFilterEnum.Stop;
			_nextButton.MouseFilter = MouseFilterEnum.Stop;
		}
	}
}
