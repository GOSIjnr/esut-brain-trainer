using Godot;
using Godot.Collections;

namespace GOSIjnr;

[GlobalClass]
public partial class Data : Node
{
	[Export]
	public Dictionary<Subjects, Color> SubjectColors { get; private set; } = new()
	{
		{ Subjects.Writing, new("#2BBFA9") },
		{ Subjects.Speaking, new("#EF2430") },
		{ Subjects.Reading, new("#EC1A61") },
		{ Subjects.Maths, new("#9C29A8") },
		{ Subjects.Memory, new("#E79220") },
		{ Subjects.Average, new("#69A0FB") },
	};

	[Export] public string UserDataSavePath { get; private set; } = "user://user_data.tres";
	[Export] public Color AppPrimaryColor { get; private set; } = new("#CB4154");

	[Export]
	public Dictionary<string, float> TemplateScores { get; private set; } = new()
	{
		{ "agility", 0 },
		{ "average", 0 },
		{ "aviodance", 0 },
		{ "collaspe", 0 },
		{ "memory", 0 },
		{ "pronunciation", 0 },
		{ "sound_match", 0 },
		{ "syntax", 0 },
		{ "tipping", 0 },
		{ "word_part", 0 },
	};

	public enum Subjects
	{
		Writing,
		Speaking,
		Reading,
		Maths,
		Memory,
		Average,
	}
}
