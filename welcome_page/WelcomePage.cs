public partial class WelcomePage : ContentScroller
{
	public override void _EnterTree()
	{
		base._EnterTree();
		Core.Instance.SaveManager.LoadUserData();
	}
}
