using Godot;
using GOSIjnr;
using Godot.Collections;

public partial class ProficiencyQuotient : RefCounted, IDataSerializer<Dictionary<string, Variant>>
{
	[Export] public bool IsRecommendedByUser { get; set; } = false;
	[Export] public float StartingPoints { get; set; } = 10;
	[Export] private float _currentPoints = 10;
	[Export] public Dictionary<string, float> DailyPoints { get; set; } = [];

	public float CurrentPoints
	{
		get => _currentPoints;
		set => _currentPoints = Mathf.Max(value, StartingPoints);
	}

	public Dictionary<string, Variant> SerializeObject()
	{
		return new Dictionary<string, Variant>
		{
			{ "IsRecommendedByUser", IsRecommendedByUser },
			{ "StartingPoints", StartingPoints },
			{ "CurrentPoints", CurrentPoints },
			{ "DailyPoints", DailyPoints }
		};
	}

	public void DeserializeObject(Dictionary<string, Variant> data)
	{
		IsRecommendedByUser = data.GetValueOrDefault("IsRecommendedByUser", () => false);
		StartingPoints = data.GetValueOrDefault("StartingPoints", () => 10f);
		_currentPoints = data.GetValueOrDefault("CurrentPoints", () => StartingPoints);
		DailyPoints = data.GetValueOrDefault("DailyPoints", () => new Dictionary<string, float>());
	}
}
