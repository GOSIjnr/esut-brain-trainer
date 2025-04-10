using Godot;
using System;

public partial class TouchInputHandler(Vector2 boundSize) : Node
{
	private Rect2 _activeTouchRect;
	private Vector2 _boundSize = boundSize;

	public event Action<Vector2> TouchStarted;
	public event Action TouchCompleted;

	public void HandleTouchInput(InputEvent @event)
	{
		if (@event is not InputEventScreenTouch touchInput) return;

		if (touchInput.IsPressed())
		{
			InitializeTouchBoundary(touchInput.Position);
			TouchStarted?.Invoke(touchInput.Position);
			return;
		}

		if (touchInput.IsReleased() && IsTouchReleaseValid(touchInput.Position))
		{
			TouchCompleted?.Invoke();
		}
	}

	private void InitializeTouchBoundary(Vector2 position)
	{
		Vector2 boundaryPosition = position - (_boundSize / 2);
		_activeTouchRect = new Rect2(boundaryPosition, _boundSize);
	}

	private bool IsTouchReleaseValid(Vector2 position) => _activeTouchRect.HasPoint(position);
}
