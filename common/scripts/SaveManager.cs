using Godot;
using GOSIjnr;
using Godot.Collections;

[GlobalClass]
public partial class SaveManager : Node
{
	[Export] public UserData UserProfileData { get; set; }
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
		UserProfileData = new();

		var newScores = Utils.MergeDictionaries(UserProfileData.HighScores, Core.Instance.AppData.TemplateScores);

		UserProfileData.HighScores = new Dictionary<string, float>(newScores);
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
			Logger.Log("Failed to parse JSON data", Logger.LogLevel.Warning);
			CreateUserData();
			return;
		}

		if (JSON.Data.VariantType != Variant.Type.Dictionary)
		{
			Logger.Log("Parsed JSON is not a dictionary", Logger.LogLevel.Warning);
			CreateUserData();
			return;
		}

		var jsonData = JSON.Data.AsGodotDictionary<string, Variant>();

		UserProfileData = new UserData();
		UserProfileData.DeserializeObject(jsonData);
	}

	public void SaveUserData()
	{
		UserProfileData.SubjectProficiencies[AppData.Subjects.Average.ToString()].CurrentPoints = UserStatsService.ComputeAverageStartingPoint(UserProfileData);

		var jsonData = Json.Stringify(UserProfileData.SerializeObject(), "\t", false);
		var file = FileAccess.Open(Core.Instance.AppData.UserDataSavePath, FileAccess.ModeFlags.Write);

		file.StoreString(jsonData);
		file.Close();
		IsUserSaveDataPresent = true;
	}

	public void EnsureUserIsDataLoaded()
	{
		if (UserProfileData == null)
		{
			LoadUserData();
		}
	}
}
