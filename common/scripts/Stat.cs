using Godot;
using System;

namespace Entity.Stats;

public abstract partial class Stat : RefCounted
{
	[Export] protected float _baseStatValue = 0;
	[Export] protected float _minimumValue = 0;
	[Export] protected float _maximumValue = 0;
	[Export] protected float _currentStatValue = 0;
	[Export] protected float _flatValueModifier = 0;
	[Export] protected float _percentageModifier = 0;

	public event Action<Stat> OnValueChanged;
	public event Action OnValueDepleted;

	public Stat(float baseStatValue, float minimumValue, float maximumValue)
	{
		_minimumValue = Mathf.Max(0, minimumValue);
		_maximumValue = Mathf.Max(minimumValue + 1, maximumValue);
		_baseStatValue = Mathf.Clamp(baseStatValue, _minimumValue, _maximumValue);

		_currentStatValue = _baseStatValue;
	}

	public float CurrentStatValue
	{
		get => _currentStatValue;
		set
		{
			_currentStatValue = Mathf.Clamp(value, _minimumValue, _maximumValue);
			OnValueChanged?.Invoke(this);

			if (_currentStatValue <= _minimumValue)
			{
				OnValueDepleted?.Invoke();
			}
		}
	}

	public float BaseStatValue
	{
		get => _baseStatValue;
		set
		{
			_baseStatValue = Mathf.Max(0, value);
			RecalculateMaximumValue();
		}
	}

	public float MinimumValue
	{
		get => _minimumValue;
		set
		{
			_minimumValue = Mathf.Clamp(value, 0, _maximumValue - 1);
			RecalculateMaximumValue();
		}
	}

	public float MaximumValue => _maximumValue;

	public float FlatValueModifier
	{
		get => _flatValueModifier;
		set
		{
			_flatValueModifier = value;
			RecalculateMaximumValue();
		}
	}

	public float PercentageModifier
	{
		get => _percentageModifier;
		set
		{
			_percentageModifier = value;
			RecalculateMaximumValue();
		}
	}

	public void ResetModifiers()
	{
		_flatValueModifier = 0;
		_percentageModifier = 0;
		RecalculateMaximumValue();
	}

	protected abstract void RecalculateMaximumValue();
}
