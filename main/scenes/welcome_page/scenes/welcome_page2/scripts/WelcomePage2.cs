using Godot;
using Godot.Collections;

public partial class WelcomePage2 : ContentPage
{
	[Export] private Array<IntroPage> _introPages = [];

	private int _currentPageIndex;
	private IntroPlayer _introPlayer;
	private IntroController _introController;

	public int CurrentPageIndex
	{
		get => _currentPageIndex;
		set
		{
			int clampedValue = Mathf.Clamp(value, 0, _introPages.Count - 1);

			if (_currentPageIndex == clampedValue) return;

			var isHigherIndex = clampedValue > _currentPageIndex;

			_currentPageIndex = clampedValue;
			UpdateUI(isHigherIndex);
		}
	}

	public override void _EnterTree()
	{
		_introPlayer = GetNode<IntroPlayer>("%IntroPlayer");
		_introController = GetNode<IntroController>("%IntroController");

		_introPlayer.AnimationPlaying += _introController.DisableControl;
		_introController.PreviousButtonClicked += DisplayPreviousPage;
		_introController.NextButtonClicked += DisplayNextPage;
	}

	public override void _ExitTree()
	{
		_introPlayer.AnimationPlaying -= _introController.DisableControl;
		_introController.PreviousButtonClicked -= DisplayPreviousPage;
		_introController.NextButtonClicked -= DisplayNextPage;
	}

	public override void _Ready()
	{
		_introController.UpdateUIForCurrentIndex(_currentPageIndex, _introPages.Count);
		CurrentPageIndex = 0;
		_introPlayer.SelectedPage = _introPages[0];
	}

	private void DisplayNextPage()
	{
		if (CurrentPageIndex + 1 >= _introPages.Count)
		{
			EmitSignalOnNextPageRequested();
			return;
		}

		CurrentPageIndex++;
	}

	private void DisplayPreviousPage() => CurrentPageIndex--;

	private void UpdateUI(bool isHigherIndex)
	{
		if (_introPlayer == null || _introController == null) return;

		_introPlayer.SelectedPage = _introPages[_currentPageIndex];

		if (isHigherIndex)
		{
			_ = _introPlayer.NextAnimation();
		}
		else
		{
			_ = _introPlayer.PreviousAnimation();
		}

		_introController.UpdateUIForCurrentIndex(_currentPageIndex, _introPages.Count);
	}
}
