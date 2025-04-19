using Godot;
using System.Threading.Tasks;

namespace GOSIjnr;

public partial class Page2Player : Control
{
	private IntroPage _selectedPage;

	public IntroPage SelectedPage
	{
		get => _selectedPage;
		set
		{
			_selectedPage = value;
			_introImage.Texture = value.IntroTexture;
			_introHeading.Text = value.IntroTitle;
			_introBody.Text = value.IntroBody;
		}
	}

	private TextureRect _introImage;
	private Label _introHeading;
	private Label _introBody;

	[Signal] public delegate void AnimationPlayingEventHandler(bool isPlaying);

	public override void _EnterTree()
	{
		_introImage = GetNodeOrNull<TextureRect>("%Image");
		_introHeading = GetNodeOrNull<Label>("%Heading");
		_introBody = GetNodeOrNull<Label>("%Body");
	}

	public async Task NextAnimation()
	{
		await PlayAnimation(0, -Size[0]);
		await PlayAnimation(Size[0], 0);
	}

	public async Task PreviousAnimation()
	{
		await PlayAnimation(0, Size[0]);
		await PlayAnimation(-Size[0], 0);
	}


	private async Task PlayAnimation(float currentPosition, float targetPosition)
	{
		EmitSignalAnimationPlaying(true);
		Position = new Vector2(currentPosition, 0);

		var positionNodePath = new NodePath(PropertyName.Position).GetAsPropertyPath();

		var tween = CreateTween();
		tween.TweenProperty(this, positionNodePath, new Vector2(targetPosition, 0), 0.15f).SetEase(Tween.EaseType.Out);
		await ToSignal(tween, Tween.SignalName.Finished);
		EmitSignalAnimationPlaying(false);
	}
}
