using Godot;
using Godot.Collections;

namespace GOSIjnr;

public partial class WelcomePage2 : ContentPage
{
	[Export] private Array<IntroPage> _introPages = [];

	private int _currentPageIndex;
	private Page2Player _page2Player;
	private Page2Controller _page2Controller;

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
		_page2Player = GetNode<Page2Player>("%Page2Player");
		_page2Controller = GetNode<Page2Controller>("%Page2Controller");

		_page2Player.AnimationPlaying += _page2Controller.DisableControl;
		_page2Controller.PreviousButtonClicked += DisplayPreviousPage;
		_page2Controller.NextButtonClicked += DisplayNextPage;
	}

	public override void _ExitTree()
	{
		_page2Player.AnimationPlaying -= _page2Controller.DisableControl;
		_page2Controller.PreviousButtonClicked -= DisplayPreviousPage;
		_page2Controller.NextButtonClicked -= DisplayNextPage;
	}

	public override void _Ready()
	{
		_page2Controller.UpdateUIForCurrentIndex(_currentPageIndex, _introPages.Count);
		CurrentPageIndex = 0;
		_page2Player.SelectedPage = _introPages[0];
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
		if (_page2Player == null || _page2Controller == null) return;

		_page2Player.SelectedPage = _introPages[_currentPageIndex];

		if (isHigherIndex)
		{
			_ = _page2Player.NextAnimation();
		}
		else
		{
			_ = _page2Player.PreviousAnimation();
		}

		_page2Controller.UpdateUIForCurrentIndex(_currentPageIndex, _introPages.Count);
	}
}
