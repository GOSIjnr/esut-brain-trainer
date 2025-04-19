using Godot;

[GlobalClass]
public partial class IntroPage : Resource
{
	[Export] public Texture2D IntroTexture { get; private set; }
	[Export] public string IntroTitle { get; private set; }
	[Export(PropertyHint.MultilineText)] public string IntroBody { get; private set; }
}
