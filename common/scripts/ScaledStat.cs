using Godot;

namespace Entity.Stats;

public partial class ScaledStat(float baseStatValue, float minimumValue, float maximumValue) : Stat(baseStatValue, minimumValue, maximumValue)
{
	protected override void RecalculateMaximumValue()
	{
		var newMaxValue = _baseStatValue * (1 + _percentageModifier / 100) + _flatValueModifier;

		_maximumValue = newMaxValue;
		_currentStatValue = Mathf.Clamp(_currentStatValue, _minimumValue, _maximumValue);
	}
}
