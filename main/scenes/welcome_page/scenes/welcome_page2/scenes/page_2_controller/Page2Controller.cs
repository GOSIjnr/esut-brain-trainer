using Godot;

namespace GOSIjnr;

public partial class Page2Controller : MarginContainer
{
	private Button _previousButton;
	private Button _nextButton;

	private bool _previousButtonState;
	private bool _nextButtonState;

	[Signal] public delegate void PreviousButtonClickedEventHandler();
	[Signal] public delegate void NextButtonClickedEventHandler();

	public override void _EnterTree()
	{
		_previousButton = GetNodeOrNull<Button>("%Button1");
		_nextButton = GetNodeOrNull<Button>("%Button2");

		if (_previousButton == null || _nextButton == null) return;

		_previousButton.Pressed += PreviousPage;
		_nextButton.Pressed += NextPage;
	}

	public override void _ExitTree()
	{
		_previousButton.Pressed -= PreviousPage;
		_nextButton.Pressed -= NextPage;
	}

	public void PreviousPage()
	{
		EmitSignalPreviousButtonClicked();
	}

	public void NextPage()
	{
		EmitSignalNextButtonClicked();
	}

	public void UpdateUIForCurrentIndex(int currentIndex, int totalCount)
	{
		if (_previousButton == null || _nextButton == null) return;

		_previousButton.Disabled = false;
		_nextButton.Disabled = false;
		_nextButton.Text = "NEXT";

		if (currentIndex <= 0)
		{
			_previousButton.Disabled = true;
		}

		if (currentIndex >= totalCount - 1)
		{
			_nextButton.Text = "CONTINUE";
		}
	}

	public void DisableControl(bool isDisable)
	{
		if (_previousButton == null || _nextButton == null) return;

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
