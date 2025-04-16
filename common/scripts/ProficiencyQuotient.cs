using Godot;
using GOSIjnr;
using Godot.Collections;

public partial class ProficiencyQuotient : RefCounted, IDataSerializer<Dictionary<string, Variant>>
{
	[Export] public bool isRecommendedByUser;
	[Export] public float startingPoints = 10;
	[Export] private float _currentPoints = 10;
	[Export] public Dictionary<string, float> dailyPoints = [];

	public float CurrentPoints
	{
		get => _currentPoints;
		set => _currentPoints = Mathf.Max(value, startingPoints);
	}

	public Dictionary<string, Variant> SerializeObject()
	{
		return new Dictionary<string, Variant>
		{
			{ "IsRecommendedByUser", isRecommendedByUser },
			{ "StartingPoints", startingPoints },
			{ "CurrentPoints", CurrentPoints },
			{ "DailyPoints", dailyPoints }
		};
	}

	public void DeserializeObject(Dictionary<string, Variant> data)
	{
		isRecommendedByUser = data.GetValueOrDefault("IsRecommendedByUser", () => false);
		startingPoints = data.GetValueOrDefault("StartingPoints", () => 10f);
		_currentPoints = data.GetValueOrDefault("CurrentPoints", () => startingPoints);
		dailyPoints = data.GetValueOrDefault("DailyPoints", () => new Dictionary<string, float>());
	}
}
