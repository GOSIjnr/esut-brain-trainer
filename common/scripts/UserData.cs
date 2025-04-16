using Godot;
using GOSIjnr;
using Godot.Collections;

public partial class UserData : RefCounted, IDataSerializer<Dictionary<string, Variant>>
{
	[Export] public string UserName { get; set; } = "User";
	[Export] public bool IsTutorialDone { get; set; }
	[Export] public int Workouts { get; set; }
	[Export] public Dictionary<string, float> HighScores { get; set; } = [];
	[Export] public ProficiencyQuotient Writing { get; set; } = new();
	[Export] public ProficiencyQuotient Speaking { get; set; } = new();
	[Export] public ProficiencyQuotient Reading { get; set; } = new();
	[Export] public ProficiencyQuotient Maths { get; set; } = new();
	[Export] public ProficiencyQuotient Memory { get; set; } = new();
	[Export] public ProficiencyQuotient Average { get; set; } = new();

	public Dictionary<string, Variant> SerializeObject()
	{
		return new Dictionary<string, Variant>
		{
			{ "UserName", UserName },
			{ "IsTutorialDone", IsTutorialDone },
			{ "Workouts", Workouts },
			{ "HighScores", HighScores },
			{ "Writing", Writing.SerializeObject() },
			{ "Speaking", Speaking.SerializeObject() },
			{ "Reading", Reading.SerializeObject() },
			{ "Maths", Maths.SerializeObject() },
			{ "Memory", Memory.SerializeObject() },
			{ "Average", Average.SerializeObject() },
		};
	}

	public void DeserializeObject(Dictionary<string, Variant> data)
	{
		UserName = data.GetValueOrDefault("UserName", () => "User");
		IsTutorialDone = data.GetValueOrDefault("IsTutorialDone", () => false);
		Workouts = data.GetValueOrDefault("Workouts", () => 0);
		HighScores = data.GetValueOrDefault("HighScores", () => new Dictionary<string, float>());
		Writing.DeserializeObject(data.GetValueOrDefault("Writing", () => new Dictionary<string, Variant>()));
		Speaking.DeserializeObject(data.GetValueOrDefault("Speaking", () => new Dictionary<string, Variant>()));
		Reading.DeserializeObject(data.GetValueOrDefault("Reading", () => new Dictionary<string, Variant>()));
		Maths.DeserializeObject(data.GetValueOrDefault("Maths", () => new Dictionary<string, Variant>()));
		Memory.DeserializeObject(data.GetValueOrDefault("Memory", () => new Dictionary<string, Variant>()));
		Average.DeserializeObject(data.GetValueOrDefault("Average", () => new Dictionary<string, Variant>()));
	}
}
