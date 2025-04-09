using Godot;
using System.Linq;
using Godot.Collections;

namespace GOSIjnr;

[GlobalClass]
public partial class SaveManager : Node
{
	[Export] public UserData userData;
	[Export] public bool IsUserSaveDataPresent { get; private set; }

	public override void _EnterTree()
	{
		CheckUserData();
	}

	private void CheckUserData()
	{
		IsUserSaveDataPresent = FileAccess.FileExists(Core.Instance.Data.UserDataSavePath);
	}

	public void CreateUserData()
	{
		userData = new();

		var newScores = Utils.MergeDictionaries(userData.highScores, Core.Instance.Data.TemplateScores);
		userData.highScores.Clear();

		foreach (var score in newScores)
		{
			userData.highScores.Add(score.Key, score.Value);
		}
	}

	public void LoadUserData()
	{
		if (!FileAccess.FileExists(Core.Instance.Data.UserDataSavePath))
		{
			CreateUserData();
			return;
		}

		var file = FileAccess.Open(Core.Instance.Data.UserDataSavePath, FileAccess.ModeFlags.Read);
		var jsonString = file.GetAsText();
		file.Close();


		var jsonData = (Dictionary<string, Variant>)Json.ParseString(jsonString);

		if (jsonData == null)
		{
			Logger.Log("Failed to parse JSON data");
			CreateUserData();
			return;
		}

		userData = new UserData();
		userData.ApplyData(jsonData);
	}

	public void SaveUserData()
	{
		var scores = new System.Collections.Generic.List<float>
		{
			userData.writing.CurrentPoints,
			userData.reading.CurrentPoints,
			userData.speaking.CurrentPoints,
			userData.maths.CurrentPoints,
			userData.memory.CurrentPoints
		};

		var averageScore = scores.Sum() / scores.Count;
		userData.average.CurrentPoints = averageScore;

		var jsonData = Json.Stringify(userData.GetData(), "\t", false);
		var file = FileAccess.Open(Core.Instance.Data.UserDataSavePath, FileAccess.ModeFlags.Write);
		file.StoreString(jsonData);
		file.Close();
		IsUserSaveDataPresent = true;
	}
}
