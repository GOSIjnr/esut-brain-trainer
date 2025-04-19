using System.Linq;

public static class UserStatsService
{
	public static float ComputeUserAverageCurrentPoint(UserData data)
	{
		return data.SubjectProficiencies
			.Where(kv => kv.Key != AppData.Subjects.Average.ToString())
			.Select(kv => kv.Value.CurrentPoints)
			.Average();
	}

	public static float ComputeAverageStartingPoint(UserData data)
	{
		return data.SubjectProficiencies
			.Where(kv => kv.Key != AppData.Subjects.Average.ToString())
			.Select(kv => kv.Value.StartingPoints)
			.Average();
	}
}
