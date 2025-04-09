using Godot;

namespace GOSIjnr;

[GlobalClass]
public abstract partial class Question : Resource
{
	[Export(PropertyHint.MultilineText)] public string QuestionText { get; private set; }

}
