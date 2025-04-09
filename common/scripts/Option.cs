using Godot;

namespace GOSIjnr;

[GlobalClass]
public abstract partial class Option : Resource
{
	[Export(PropertyHint.MultilineText)] public string OptionText { get; private set; }
	[Export] public float Points { get; private set; }
}
