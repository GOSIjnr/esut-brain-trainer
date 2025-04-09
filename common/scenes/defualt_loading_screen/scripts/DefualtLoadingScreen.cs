public partial class DefualtLoadingScreen : LoadingScreen
{
	public override void HandleSceneLoadingCompleted()
	{
		QueueFree();
	}
}
