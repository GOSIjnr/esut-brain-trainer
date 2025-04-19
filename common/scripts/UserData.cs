using Godot;
using GOSIjnr;
using System.Linq;
using Godot.Collections;

public partial class UserData : RefCounted, IDataSerializer<Dictionary<string, Variant>>
{
	[Export] public string UserName { get; set; } = "User";
	[Export] public bool IsTutorialDone { get; set; }
	[Export] public int Workouts { get; set; }
	[Export] public Dictionary<string, float> HighScores { get; set; } = [];
	[Export]
	public Dictionary<string, ProficiencyQuotient> SubjectProficiencies { get; set; } = new()
	{
		{ AppData.Subjects.Writing.ToString() , new ProficiencyQuotient() },
		{ AppData.Subjects.Speaking.ToString(), new ProficiencyQuotient() },
		{ AppData.Subjects.Reading.ToString(), new ProficiencyQuotient() },
		{ AppData.Subjects.Maths.ToString(), new ProficiencyQuotient() },
		{ AppData.Subjects.Memory.ToString(), new ProficiencyQuotient() },
		{ AppData.Subjects.Average.ToString(), new ProficiencyQuotient() },
	};

	public Dictionary<string, Variant> SerializeObject()
	{
		var subjectData = new Dictionary<string, Variant>();

		foreach (var subject in SubjectProficiencies)
		{
			subjectData.Add(subject.Key, subject.Value.SerializeObject());
		}

		return new()
		{
			{ "UserName", UserName },
			{ "IsTutorialDone", IsTutorialDone },
			{ "Workouts", Workouts },
			{ "HighScores", HighScores },
			{ "Proficiencies", subjectData },
		};
	}

	public void DeserializeObject(Dictionary<string, Variant> data)
	{
		UserName = data.GetValueOrDefault("UserName", () => "User");
		IsTutorialDone = data.GetValueOrDefault("IsTutorialDone", () => false);
		Workouts = data.GetValueOrDefault("Workouts", () => 0);
		HighScores = data.GetValueOrDefault("HighScores", () => new Dictionary<string, float>());

		if (data.TryGetValue("Proficiencies", out var proficienciesData) && proficienciesData.Obj is Dictionary proficiencyData)
		{
			foreach (string key in proficiencyData.Keys.Select(v => (string)v))
			{
				if (SubjectProficiencies.TryGetValue(key, out ProficiencyQuotient value))
				{
					try
					{
						var userProficiencyMap = proficiencyData[key].As<Dictionary<string, Variant>>();
						SubjectProficiencies[key].DeserializeObject(userProficiencyMap);
					}
					catch (System.InvalidCastException e)
					{
						GD.PrintErr($"Proficiency data for '{key}' is not a Dictionary<string, Variant>: {e.Message}");
					}
					catch (System.Exception e)
					{
						GD.PrintErr($"Unexpected error while processing proficiency '{key}': {e.Message}");
					}
				}
			}
		}
	}
}
