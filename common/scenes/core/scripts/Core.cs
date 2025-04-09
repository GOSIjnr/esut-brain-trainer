using Godot;
using GOSIjnr;

[GlobalClass]
public partial class Core : Node
{
	public CrashHandlerManager CrashHandler { get; private set; }
	public EventBus EventBus { get; private set; }
	public SceneManager SceneManager { get; private set; }
	public ToastManager ToastManager { get; private set; }
	public Data Data { get; private set; }
	public SaveManager SaveManager { get; private set; }

	public static Core Instance { get; private set; }

	public override void _EnterTree()
	{
		Instance = this;
		CrashHandler = GetNode<CrashHandlerManager>("%CrashHandlerManager");
		EventBus = GetNode<EventBus>("%EventBus");
		SceneManager = GetNode<SceneManager>("%SceneManager");
		ToastManager = GetNode<ToastManager>("%ToastManager");

		SetupDebugMode();
		SubscribeToEvents();
		ConfigureEngine();
	}

	public override void _ExitTree()
	{
		Logger.OnFatalError -= HandleFatalError;
		GetTree().Root.ChildExitingTree -= HandleEventCleanups;
	}

	private void SubscribeToEvents()
	{
		EventBus.Subscribe("GameCrashed", CrashHandler.HandleCrash);

		Logger.OnFatalError += HandleFatalError;
		GetTree().Root.ChildExitingTree += HandleEventCleanups;
	}

	private void SetupDebugMode()
	{
		if (!OS.HasFeature("debug")) return;

		Logger.SetLogLevelFilter(Logger.LogLevel.Debug);
	}

	private void ConfigureEngine()
	{
		float refreshRate = DisplayServer.ScreenGetRefreshRate();
		refreshRate = (refreshRate < 0.0f) ? 60.0f : refreshRate;
		Engine.MaxFps = Mathf.FloorToInt(refreshRate);
	}

	private void HandleFatalError() => EventBus.Publish("GameCrashed");

	private void HandleEventCleanups(Node node) => EventBus.CleanupEvents();
}
