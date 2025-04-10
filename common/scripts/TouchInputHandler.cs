using Godot;

public partial class TouchInputHandler(Vector2 boundSize) : Node
{
	private Rect2 _activeTouchRect;
	private Vector2 _boundSize = boundSize;

	[Signal] public delegate void TouchStartedEventHandler(Vector2 position);
	[Signal] public delegate void TouchCompletedEventHandler();

	public void HandleTouchInput(InputEvent @event)
	{
		if (@event is not InputEventScreenTouch touchInput) return;

		if (touchInput.IsPressed())
		{
			InitializeTouchBoundary(touchInput.Position);
			EmitSignalTouchStarted(touchInput.Position);
			return;
		}

		if (touchInput.IsReleased() && IsTouchReleaseValid(touchInput.Position))
		{
			EmitSignalTouchCompleted();
		}
	}

	private void InitializeTouchBoundary(Vector2 position)
	{
		Vector2 boundaryPosition = position - (_boundSize / 2);
		_activeTouchRect = new Rect2(boundaryPosition, _boundSize);
	}

	private bool IsTouchReleaseValid(Vector2 position) => _activeTouchRect.HasPoint(position);
}
