using Godot;
using Godot.Collections;

namespace GOSIjnr
{
	[GlobalClass]
	public partial class UserData : Resource
	{
		[Export] public string userName = "User";
		[Export] public bool isTutorialDone;
		[Export] public int workouts;
		[Export] public Dictionary<string, float> highScores = new();
		[Export] public PrionfenyQuotient writing = new();
		[Export] public PrionfenyQuotient speaking = new();
		[Export] public PrionfenyQuotient reading = new();
		[Export] public PrionfenyQuotient maths = new();
		[Export] public PrionfenyQuotient memory = new();
		[Export] public PrionfenyQuotient average = new();

		/// <summary>
		/// Updates properties of UserData from the provided dictionary.
		/// </summary>
		/// <param name="newValue">A dictionary containing key-value pairs to update.</param>
		public void ApplyData(Dictionary<string, Variant> newValue)
		{
			foreach (var pair in newValue)
			{
				switch (pair.Key)
				{
					case "user_name":
						userName = (string)pair.Value;
						break;
					case "is_tutorial_done":
						isTutorialDone = (bool)pair.Value;
						break;
					case "workouts":
						workouts = (int)pair.Value;
						break;
					case "high_scores":
						highScores = (Dictionary<string, float>)pair.Value;
						break;
					case "writing":
						writing.ApplyData((Dictionary<string, Variant>)pair.Value);
						break;
					case "speaking":
						speaking.ApplyData((Dictionary<string, Variant>)pair.Value);
						break;
					case "reading":
						reading.ApplyData((Dictionary<string, Variant>)pair.Value);
						break;
					case "maths":
						maths.ApplyData((Dictionary<string, Variant>)pair.Value);
						break;
					case "memory":
						memory.ApplyData((Dictionary<string, Variant>)pair.Value);
						break;
					case "average":
						average.ApplyData((Dictionary<string, Variant>)pair.Value);
						break;
				}
			}
		}

		/// <summary>
		/// Retrieves the current state of UserData as a dictionary.
		/// </summary>
		/// <returns>A dictionary representing the current state.</returns>
		public Dictionary<string, Variant> GetData()
		{
			return new Dictionary<string, Variant>
			{
				{ "user_name", userName },
				{ "is_tutorial_done", isTutorialDone },
				{ "workouts", workouts },
				{ "high_scores", highScores },
				{ "writing", writing.GetData() },
				{ "speaking", speaking.GetData() },
				{ "reading", reading.GetData() },
				{ "maths", maths.GetData() },
				{ "memory", memory.GetData() },
				{ "average", average.GetData() },
			};
		}

		public partial class PrionfenyQuotient : Resource
		{
			[Export] public bool isRecommendedByUser;
			[Export] public float startingPoints = 10;
			[Export] private float _currentPoints = 10;
			[Export] public Dictionary<string, float> dailyPoints = [];

			/// <summary>
			/// Gets or sets the current points ensuring they never fall below startingPoints.
			/// </summary>
			public float CurrentPoints
			{
				get => _currentPoints;
				set => _currentPoints = Mathf.Max(value, startingPoints);
			}

			/// <summary>
			/// Applies new data from a dictionary to update properties.
			/// </summary>
			/// <param name="newValue">A dictionary containing key-value pairs to update.</param>
			public void ApplyData(Dictionary<string, Variant> newValue)
			{
				foreach (var pair in newValue)
				{
					switch (pair.Key)
					{
						case "is_recommended_by_user":
							isRecommendedByUser = (bool)pair.Value;
							break;
						case "starting_points":
							startingPoints = (float)pair.Value;
							break;
						case "current_points":
							_currentPoints = (float)pair.Value;
							break;
						case "daily_points":
							dailyPoints = (Dictionary<string, float>)pair.Value;
							break;
					}
				}
			}

			/// <summary>
			/// Retrieves the current state of PrionfenyQuotient as a dictionary.
			/// </summary>
			/// <returns>A dictionary representing the current state.</returns>
			public Dictionary<string, Variant> GetData()
			{
				return new Dictionary<string, Variant>
				{
					{ "is_recommended_by_user", isRecommendedByUser },
					{ "starting_points", startingPoints },
					{ "current_points", _currentPoints },
					{ "daily_points", dailyPoints },
				};
			}
		}
	}
}