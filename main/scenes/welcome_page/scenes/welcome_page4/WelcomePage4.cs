using Godot;
using Godot.Collections;

public partial class WelcomePage4 : ContentPage
{
	private Array<Button> _toggleButtons = [];
	private GridContainer _holder;
	private Button _nextButton;
	private bool _isNextButtonLocked = true;

	public bool IsNextButtonLocked
	{
		get => _isNextButtonLocked;
		private set
		{
			_isNextButtonLocked = value;

			if (_isNextButtonLocked)
			{
				_nextButton.Disabled = true;
				return;
			}

			_nextButton.Disabled = false;
		}
	}

	public override void _EnterTree()
	{
		_holder = GetNodeOrNull<GridContainer>("%GridContainer");
		_nextButton = GetNodeOrNull<Button>("%Button");

		if (_holder == null || _nextButton == null) return;

		_nextButton.Pressed += NextPage;
	}

	public override void _ExitTree()
	{
		if (_holder == null || _nextButton == null) return;

		_nextButton.Pressed -= NextPage;

		foreach (var button in _toggleButtons)
		{
			button.Toggled -= CheckValidSelection;
		}
	}

	public override void _Ready()
	{
		if (_holder == null || _nextButton == null) return;

		IsNextButtonLocked = true;

		foreach (var child in _holder.GetChildren())
		{
			if (child is Button button)
			{
				button.ToggleMode = true;
				_toggleButtons.Add(button);
				button.Toggled += CheckValidSelection;
			}
		}
	}

	private void NextPage()
	{
		SaveSelectedOptions();
		EmitSignalOnNextPageRequested();
	}

	private void SaveSelectedOptions()
	{
		var userData = Core.Instance.SaveManager.UserProfileData;

		// userData.writing.isRecommendedByUser = _toggleButtons[0].ButtonPressed;
		// userData.speaking.isRecommendedByUser = _toggleButtons[1].ButtonPressed;
		// userData.reading.isRecommendedByUser = _toggleButtons[2].ButtonPressed;
		// userData.maths.isRecommendedByUser = _toggleButtons[3].ButtonPressed;
		// userData.memory.isRecommendedByUser = _toggleButtons[4].ButtonPressed;

		Core.Instance.SaveManager.SaveUserData();
	}

	private void CheckValidSelection(bool isToogled)
	{
		foreach (var button in _toggleButtons)
		{
			if (button.ButtonPressed)
			{
				IsNextButtonLocked = false;
				return;
			}

			IsNextButtonLocked = true;
		}
	}
}
