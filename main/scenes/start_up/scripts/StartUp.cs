using Godot;
using GOSIjnr;

public partial class StartUp : CanvasLayer
{
	public override void _Ready()
	{
		var saveManager = Core.Instance.SaveManager;

		if (saveManager.IsUserSaveDataPresent)
		{
			saveManager.LoadUserData();

			var data = saveManager.UserProfileData;
			var newScores = Utils.MergeDictionaries(data.HighScores, Core.Instance.AppData.TemplateScores);
			data.HighScores.Clear();

			foreach (var score in newScores)
			{
				data.HighScores.Add(score.Key, score.Value);
			}

			saveManager.SaveUserData();
		}
		else
		{
			saveManager.CreateUserData();
			saveManager.SaveUserData();
		}

		if (saveManager.UserProfileData.IsTutorialDone)
		{
			Core.Instance.SceneManager.SwitchScene("MainMenu", "None");
		}
		else
		{
			Core.Instance.SceneManager.SwitchScene("WelcomePage", "None");
		}
	}
}
