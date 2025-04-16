public partial class DefaultLoadingScreen : LoadingScreen
{
	public override void HandleSceneLoadingCompleted()
	{
		EmitSignalOnUnloadRequested(this);
	}
}
