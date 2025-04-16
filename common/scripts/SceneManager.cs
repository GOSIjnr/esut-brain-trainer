using Godot;
using System.Threading;
using System.Threading.Tasks;
using System.Collections.Generic;

[GlobalClass]
public partial class SceneManager : Node
{
	[Export] private PackedScene _defaultLoadingPackedScene;

	private readonly Dictionary<string, string> _scenePaths = [];
	private readonly Dictionary<string, string> _loadingScreenPaths = [];

	private LoadingScreen _currentLoadingScreen;
	private LoadingScreen _fallbackLoadingScreen;
	private CancellationTokenSource _loadCancellationTokenSource;
	private Godot.Collections.Array _loadProgressArray = [];
	private bool _useSubThreads = true;

	[Signal] public delegate void SceneLoadingStartedEventHandler();
	[Signal] public delegate void SceneLoadingProgressUpdatedEventHandler(float progress);
	[Signal] public delegate void SceneLoadingCompletedEventHandler();

	public override void _EnterTree()
	{
		bool prevBatchMode = Logger.IsBatchModeEnabled;
		Logger.IsBatchModeEnabled = true;

		foreach (var sceneEntry in SceneRegistry.SceneResourcePaths)
		{
			RegisterScene(sceneEntry.Key, sceneEntry.Value);
		}

		foreach (var loadingEntry in SceneRegistry.LoadingSceneResourcePaths)
		{
			RegisterLoadingScreen(loadingEntry.Key, loadingEntry.Value);
		}

		Logger.IsBatchModeEnabled = prevBatchMode;

		if (_defaultLoadingPackedScene == null)
		{
			Logger.Log("Default loading scene not set. Assign a valid PackedScene in the inspector.", Logger.LogLevel.Fatal);
			return;
		}

		var sceneInstance = _defaultLoadingPackedScene.Instantiate();

		if (sceneInstance is not LoadingScreen loadingScreenInstance)
		{
			sceneInstance.QueueFree();
			Logger.Log("Default loading scene instance does not derive from LoadingScreen. Check _defaultLoadingPackedScene.", Logger.LogLevel.Fatal);
			return;
		}

		_fallbackLoadingScreen = loadingScreenInstance;
	}

	public override void _ExitTree()
	{
		_fallbackLoadingScreen.QueueFree();
		_fallbackLoadingScreen = null;
	}

	public void SwitchScene(string sceneId, string loadingScreenId = "")
	{
		if (!_scenePaths.TryGetValue(sceneId, out string targetScenePath))
		{
			Logger.Log($"Scene '{sceneId}' not found. Available scenes: {string.Join(", ", _scenePaths.Keys)}.", Logger.LogLevel.Warning);
			return;
		}

		SetCurrentLoadingScreen(loadingScreenId);
		_ = LoadSceneAsync(sceneId, targetScenePath);
	}

	private void SetCurrentLoadingScreen(string loadingScreenId)
	{
		LoadingScreen CreateFallback() => (LoadingScreen)_fallbackLoadingScreen.Duplicate();

		if (!_loadingScreenPaths.TryGetValue(loadingScreenId, out var loadingPath))
		{
			Logger.Log($"Invalid loading screen ID '{loadingScreenId}'. Using fallback loading screen.", Logger.LogLevel.Warning);
			UpdateCurrentLoadingScreen(CreateFallback());
			return;
		}

		var loadingResource = ResourceLoader.Load(loadingPath);

		if (loadingResource is not PackedScene loadingPackedScene)
		{
			Logger.Log($"Cannot load PackedScene from '{loadingPath}' for loading ID '{loadingScreenId}'. Using fallback.", Logger.LogLevel.Warning);
			UpdateCurrentLoadingScreen(CreateFallback());
			return;
		}

		var loadingInstance = loadingPackedScene.Instantiate();

		if (loadingInstance is not LoadingScreen loadingScreen)
		{
			Logger.Log("Loaded resource is not a LoadingScreen instance. Using fallback.", Logger.LogLevel.Warning);
			loadingInstance.QueueFree();
			UpdateCurrentLoadingScreen(CreateFallback());
			return;
		}

		UpdateCurrentLoadingScreen(loadingScreen);
	}

	private void UpdateCurrentLoadingScreen(LoadingScreen newScreen)
	{
		_currentLoadingScreen?.QueueFree();
		_currentLoadingScreen = newScreen;
		AttachCurrentLoadingScreen();
	}

	private void AttachCurrentLoadingScreen()
	{
		GetTree().Root.CallDeferred(MethodName.AddChild, _currentLoadingScreen);
		_currentLoadingScreen.AttachToSceneManager(this);
	}

	private async Task LoadSceneAsync(string sceneId, string scenePath)
	{
		Logger.Log($"Starting async load for scene at {sceneId} ({scenePath}).", Logger.LogLevel.Debug);
		EmitSignalSceneLoadingStarted();

		_loadCancellationTokenSource?.Cancel();
		_loadCancellationTokenSource = new CancellationTokenSource();
		CancellationToken token = _loadCancellationTokenSource.Token;

		var loadRequest = ResourceLoader.LoadThreadedRequest(scenePath, "", _useSubThreads);

		if (loadRequest != Error.Ok)
		{
			Logger.Log($"Failed to start threaded load for {sceneId} ({scenePath}).", Logger.LogLevel.Debug);
			return;
		}

		var threadLoadStatus = ResourceLoader.LoadThreadedGetStatus(scenePath, _loadProgressArray);
		await ToSignal(GetTree(), SceneTree.SignalName.ProcessFrame);

		while (threadLoadStatus == ResourceLoader.ThreadLoadStatus.InProgress)
		{
			threadLoadStatus = ResourceLoader.LoadThreadedGetStatus(scenePath, _loadProgressArray);

			if (token.IsCancellationRequested)
			{
				Logger.Log($"Load for {sceneId} ({scenePath}) was cancelled.", Logger.LogLevel.Debug);
				return;
			}

			EmitSignalSceneLoadingProgressUpdated((float)_loadProgressArray[0]);
			await ToSignal(GetTree(), SceneTree.SignalName.ProcessFrame);
		}

		if (ResourceLoader.LoadThreadedGetStatus(scenePath) != ResourceLoader.ThreadLoadStatus.Loaded)
		{
			Logger.Log($"Scene load for {sceneId} ({scenePath}) did not complete successfully.", Logger.LogLevel.Error);
		}

		EmitSignalSceneLoadingProgressUpdated((float)_loadProgressArray[0]);
		Logger.Log($"Scene at {sceneId} ({scenePath}) loaded. Changing scene.....", Logger.LogLevel.Debug);

		var loadedResource = ResourceLoader.LoadThreadedGet(scenePath);

		if (loadedResource is not PackedScene newPackedScene)
		{
			Logger.Log($"Loaded resource at {sceneId} ({scenePath}) is not a valid PackedScene. Aborting!.", Logger.LogLevel.Fatal);
			return;
		}

		GetTree().ChangeSceneToPacked(newPackedScene);
		Logger.Log($"Active scene changed to resource at {sceneId} ({scenePath}).", Logger.LogLevel.Debug);
		EmitSignalSceneLoadingCompleted();
	}

	public void RegisterScene(string id, string path)
	{
		if (_scenePaths.TryAdd(id, path))
		{
			Logger.Log($"Registered scene '{id}' at '{path}'.", Logger.LogLevel.Debug);
		}
	}

	public void UnregisterScene(string id)
	{
		if (_scenePaths.Remove(id))
		{
			Logger.Log($"Unregistered scene '{id}'.", Logger.LogLevel.Debug);
		}
	}

	public void RegisterLoadingScreen(string id, string path)
	{
		if (_loadingScreenPaths.TryAdd(id, path))
		{
			Logger.Log($"Registered loading screen '{id}' at '{path}'.", Logger.LogLevel.Debug);
		}
	}

	public void UnregisterLoadingScreen(string id)
	{
		if (_loadingScreenPaths.Remove(id))
		{
			Logger.Log($"Unregistered loading screen '{id}'.", Logger.LogLevel.Debug);
		}
	}
}
