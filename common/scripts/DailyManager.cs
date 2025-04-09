using Godot;
using System.Linq;
using Godot.Collections;

namespace GOSIjnr;

[GlobalClass]
public partial class DailyManager : Node
{
	public void SaveDailies()
	{
		var saveManager = Core.Instance.SaveManager;

		if (saveManager == null || saveManager.userData == null) return;

		var userData = saveManager.userData;

		AddDailyData(userData.writing.dailyPoints, userData.writing.CurrentPoints);
		AddDailyData(userData.speaking.dailyPoints, userData.speaking.CurrentPoints);
		AddDailyData(userData.reading.dailyPoints, userData.reading.CurrentPoints);
		AddDailyData(userData.maths.dailyPoints, userData.maths.CurrentPoints);
		AddDailyData(userData.memory.dailyPoints, userData.memory.CurrentPoints);
		AddDailyData(userData.average.dailyPoints, userData.average.CurrentPoints);
	}

	private int DateToTimestamp(string dateString)
	{
		return (int)Time.GetUnixTimeFromDatetimeString(dateString + "T00:00:00");
	}

	private string TimeStampToDate(int timestamp)
	{
		return Time.GetDatetimeStringFromUnixTime(timestamp).Substr(0, 10);
	}

	private void AddDailyData(Dictionary<string, float> data, float CurrentPoints)
	{
		var currentDateString = Time.GetDateStringFromUnixTime((int)Time.GetUnixTimeFromSystem()).Substr(0, 10);
		var currentTimeStamp = DateToTimestamp(currentDateString);

		if (data.Count == 0)
		{
			data.Add(currentDateString, CurrentPoints);
			return;
		}

		var dataDates = data.Keys.ToArray();
		System.Array.Sort(dataDates);

		var lastDateString = dataDates.Last();
		var lastTimestamp = DateToTimestamp(lastDateString);

		if (currentTimeStamp < DateToTimestamp(dataDates[0])) return;

		if (currentTimeStamp > lastTimestamp)
		{
			var previousData = data[dataDates.Last()];

			foreach (var date in GD.Range(lastTimestamp + 86400, currentTimeStamp + 86400, 86400))
			{
				data.Add(TimeStampToDate(date), previousData);
			}

			data.Add(currentDateString, CurrentPoints);
			return;
		}

		if (currentTimeStamp == lastTimestamp)
		{
			data[currentDateString] = CurrentPoints;
			return;
		}
	}
}
