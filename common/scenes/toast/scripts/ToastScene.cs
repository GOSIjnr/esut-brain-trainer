using Godot;
using System.Threading.Tasks;

public partial class ToastScene : CanvasLayer
{
	[Export] private Vector2 _toastPadding = new(100, 60);
	[Export] private float _fadeAnimationDuration = 0.2f;

	private Panel _toastPanel;
	private Label _toastLabel;

	public override void _EnterTree()
	{
		_toastPanel = GetNode<Panel>("%Panel");
		_toastLabel = GetNode<Label>("%Label");
	}

	public async Task ShowToast(string message, float visibleDuration)
	{
		await ToSignal(this, SignalName.Ready);

		_toastLabel.Text = message;
		_toastPanel.UpdateMinimumSize();
		_toastPanel.CustomMinimumSize = _toastLabel.GetMinimumSize() + _toastPadding;

		await AnimateOpacityWithDelay(0, 1, _fadeAnimationDuration, visibleDuration);
		await AnimateOpacityWithDelay(1, 0, _fadeAnimationDuration, _fadeAnimationDuration);
		QueueFree();
	}

	private async Task AnimateOpacityWithDelay(float startOpacity, float endOpacity, float transitionDuration, float delayAfterTransition)
	{
		await AnimateOpacityTransition(startOpacity, endOpacity, transitionDuration);
		await ToSignal(GetTree().CreateTimer(delayAfterTransition), SceneTreeTimer.SignalName.Timeout);
	}

	private async Task AnimateOpacityTransition(float startOpacity, float endOpacity, float duration)
	{
		var modulatePath = new NodePath(Panel.PropertyName.Modulate).GetAsPropertyPath();
		_toastPanel.Modulate = new Color(1, 1, 1, startOpacity);

		var tween = CreateTween();
		tween.TweenProperty(_toastPanel, modulatePath, new Color(1, 1, 1, endOpacity), duration).SetEase(Tween.EaseType.In);
		await ToSignal(tween, Tween.SignalName.Finished);
	}
}
