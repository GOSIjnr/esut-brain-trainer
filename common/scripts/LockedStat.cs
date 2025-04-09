using Godot;

namespace Entity.Stats;

public partial class LockedStat(float baseStatValue, float minimumValue, float maximumValue) : Stat(baseStatValue, minimumValue, maximumValue)
{
	protected override void RecalculateMaximumValue()
	{
		var newMaxValue = _baseStatValue * (1 + _percentageModifier / 100) + _flatValueModifier;
		_currentStatValue = Mathf.Clamp(newMaxValue, _minimumValue, _maximumValue);
	}
}
