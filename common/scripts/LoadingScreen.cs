using Godot;

public abstract partial class LoadingScreen : CanvasLayer
{
	private SceneManager _activeSceneManager;

	public override void _ExitTree()
	{
		if (_activeSceneManager != null)
		{
			_activeSceneManager.SceneLoadingStarted -= HandleSceneLoadingStarted;
			_activeSceneManager.SceneLoadingProgressUpdated -= HandleSceneLoadingProgressUpdated;
			_activeSceneManager.SceneLoadingCompleted -= HandleSceneLoadingCompleted;

			_activeSceneManager = null;
		}
	}

	public void AttachToSceneManager(SceneManager sceneManager)
	{
		_activeSceneManager = sceneManager;

		_activeSceneManager.SceneLoadingStarted += HandleSceneLoadingStarted;
		_activeSceneManager.SceneLoadingProgressUpdated += HandleSceneLoadingProgressUpdated;
		_activeSceneManager.SceneLoadingCompleted += HandleSceneLoadingCompleted;
	}

	public virtual void HandleSceneLoadingStarted() { }

	public virtual void HandleSceneLoadingProgressUpdated(float progressValue) { }

	public virtual void HandleSceneLoadingCompleted() { }
}