using Godot;

[GlobalClass]
public partial class Vocabulary : Resource
{
	[Export] public string Word { get; private set; }
	[Export(PropertyHint.MultilineText)] public string Meaning { get; private set; }
}
