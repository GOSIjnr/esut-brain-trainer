using Godot;

[GlobalClass]
public partial class CrashHandlerManager : Node
{
	[Export] private PackedScene _crashScene;

	public void HandleCrash()
	{
		GetTree().Paused = true;

		if (_crashScene == null)
		{
			Logger.Log("No crash scene assigned. Please ensure _crashScene is properly configured in the editor.", Logger.LogLevel.Warning);
			GetTree().Quit();
			return;
		}

		var instance = _crashScene.Instantiate();

		if (instance is not CrashHandlerScene crashSceneInstance)
		{
			Logger.Log("Instantiation of crash scene failed. Verify that _crashScene is a valid PackedScene capable of instantiating as CrashHandlerScene.", Logger.LogLevel.Error);
			instance.QueueFree();
			GetTree().Quit();
			return;
		}

		var rootNode = GetTree().Root;

		// If the root node is available, add the crash scene instance as a deferred child,
		// This ensures node addition is safely queued. Otherwise, log an error and quit the game.
		if (rootNode != null)
		{
			rootNode.CallDeferred(MethodName.AddChild, crashSceneInstance);
			Logger.Log("Crash scene instance added to the root node successfully.", Logger.LogLevel.Debug);
		}
		else
		{
			Logger.Log("Root node of the scene tree is null. Unable to add crash scene instance. Quitting the game immediately.", Logger.LogLevel.Error);
			GetTree().Quit();
		}
	}
}
