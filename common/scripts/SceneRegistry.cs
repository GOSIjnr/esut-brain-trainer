using System.Collections.Generic;

public static class SceneRegistry
{
	public static readonly Dictionary<string, string> SceneResourcePaths = new()
	{
		{ "StartUp", "uid://bnqtkei33osav" },
		{ "WelcomePage", "uid://cksvyc04tad5i" },
		{ "Test", "uid://dmb77a7o68qmb" },
	};

	public static readonly Dictionary<string, string> LoadingSceneResourcePaths = new()
	{
		{ "None", "uid://b5bxiic22ucri" },
	};
}
