using Godot;
using Godot.Collections;

[GlobalClass]
public partial class TutorialQuestion : Question
{
	[Export] public AppData.Subjects QuestionType { get; private set; }
	[Export] public Array<TutorialOption> QuestionOptions { get; private set; } = [];
}
