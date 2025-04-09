using Godot;

[GlobalClass]
public partial class ToastManager : Node
{
	[Export] private PackedScene _toastTemplate;

	public void DisplayToastNotification(string message, float duration)
	{
		if (_toastTemplate == null)
		{
			Logger.Log("The toast template (PackedScene) is not assigned. Ensure that the Toast scene is configured properly in the editor.", Logger.LogLevel.Warning);
			return;
		}

		var toastInstanceObj = _toastTemplate.Instantiate();

		if (toastInstanceObj is not ToastScene toastInstance)
		{
			toastInstanceObj.QueueFree();
			Logger.Log("The instantiated node is not a ToastScene. Verify that _toastTemplate is set to a valid PackedScene that produces a ToastScene node.", Logger.LogLevel.Warning);
			return;
		}

		GetTree().Root.CallDeferred(MethodName.AddChild, toastInstance);
		_ = toastInstance.ShowToast(message, duration);
	}
}