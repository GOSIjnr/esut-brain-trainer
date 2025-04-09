using Godot;

namespace GOSIjnr;

/// <summary>
/// The Vocabulary resource stores a vocabulary word along with its multi-line meaning.
/// This class is marked as a global class to enable project-wide accessibility.
/// </summary>
[GlobalClass]
public partial class Vocabulary : Resource
{
	/// <summary>
	/// Gets the vocabulary word.
	/// </summary>
	[Export] public string Word { get; private set; }

	/// <summary>
	/// Gets the meaning of the vocabulary word.
	/// This property supports multi-line text input in the Godot editor.
	/// </summary>
	[Export(PropertyHint.MultilineText)] public string Meaning { get; private set; }
}
