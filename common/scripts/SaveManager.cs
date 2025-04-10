using Godot;
using GOSIjnr;
using System.Linq;

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
		IsUserSaveDataPresent = FileAccess.FileExists(Core.Instance.AppData.UserDataSavePath);
	}

	public void CreateUserData()
	{
		userData = new();

		var newScores = Utils.MergeDictionaries(userData.HighScores, Core.Instance.AppData.TemplateScores);
		userData.HighScores.Clear();

		foreach (var score in newScores)
		{
			userData.HighScores.Add(score.Key, score.Value);
		}
	}

	public void LoadUserData()
	{
		if (!FileAccess.FileExists(Core.Instance.AppData.UserDataSavePath))
		{
			CreateUserData();
			return;
		}

		var file = FileAccess.Open(Core.Instance.AppData.UserDataSavePath, FileAccess.ModeFlags.Read);
		var jsonString = file.GetAsText();
		file.Close();

		Json JSON = new();
		var error = JSON.Parse(jsonString);

		if (error != Error.Ok)
		{
			Logger.Log("Failed to parse JSON data");
			CreateUserData();
			return;
		}

		var jsonData = JSON.Data.AsGodotDictionary<string, Variant>();

		if (jsonData == null)
		{
			Logger.Log("Failed to parse JSON data");
			CreateUserData();
			return;
		}

		userData = new UserData();
		userData.DeserializeObject(jsonData);
	}

	public void SaveUserData()
	{
		var scores = new System.Collections.Generic.List<float>
		{
			userData.Writing.CurrentPoints,
			userData.Reading.CurrentPoints,
			userData.Speaking.CurrentPoints,
			userData.Maths.CurrentPoints,
			userData.Memory.CurrentPoints
		};

		var averageScore = scores.Sum() / scores.Count;
		userData.Average.CurrentPoints = averageScore;

		var jsonData = Json.Stringify(userData.SerializeObject(), "\t", false);
		var file = FileAccess.Open(Core.Instance.AppData.UserDataSavePath, FileAccess.ModeFlags.Write);

		file.StoreString(jsonData);
		file.Close();
		IsUserSaveDataPresent = true;
	}
}
