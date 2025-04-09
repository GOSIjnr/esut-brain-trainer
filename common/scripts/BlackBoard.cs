using Godot;
using Godot.Collections;

[GlobalClass]
public partial class BlackBoard : Node
{
	[Export] private Dictionary<string, Variant> _dataStorage = [];
	[Export] private EventBus _eventBus;

	public void SetData(string key, Variant data)
	{
		if (string.IsNullOrEmpty(key))
		{
			Logger.Log("SetData called with null or empty key", Logger.LogLevel.Warning);
			return;
		}

		if (_dataStorage.ContainsKey(key))
		{
			Logger.Log($"Updating existing data for key: {key} with value: {data}", Logger.LogLevel.Debug);
			_dataStorage[key] = data;
		}
		else
		{
			_dataStorage.Add(key, data);
			Logger.Log($"Adding new data for key: {key} with value: {data}", Logger.LogLevel.Debug);
		}

		_eventBus?.Publish($"OnDataChanged{key.ToPascalCase()}", data);
	}

	public T GetData<T>(string key)
	{
		if (string.IsNullOrEmpty(key))
		{
			Logger.Log("GetData called with null or empty key", Logger.LogLevel.Warning);
			return default;
		}

		if (_dataStorage.TryGetValue(key, out Variant data))
		{
			if (data.Obj is T result)
			{
				Logger.Log($"Successfully retrieved data for key: {key}", Logger.LogLevel.Debug);
				return result;
			}
			else
			{
				Logger.Log($"Error casting data for key '{key}': {data.Obj.GetType()} to {typeof(T)}", Logger.LogLevel.Error);
			}
		}
		else
		{
			Logger.Log($"Key not found in BlackBoard: {key}", Logger.LogLevel.Warning);
		}

		return default;
	}

	public bool ContainsData(string key)
	{
		if (string.IsNullOrEmpty(key))
		{
			Logger.Log("ContainsData called with null or empty key", Logger.LogLevel.Warning);
			return false;
		}

		return _dataStorage.ContainsKey(key);
	}

	public void RemoveData(string key)
	{
		if (string.IsNullOrEmpty(key))
		{
			Logger.Log("RemoveData called with null or empty key", Logger.LogLevel.Error);
			return;
		}

		bool removed = _dataStorage.Remove(key);

		if (removed)
		{
			Logger.Log($"Successfully removed data for key: {key}", Logger.LogLevel.Info);
		}
		else
		{
			Logger.Log($"Failed to remove data: key not found -> {key}", Logger.LogLevel.Warning);
		}
	}

	public void ClearAllData()
	{
		Logger.Log("Cleared all data in BlackBoard", Logger.LogLevel.Info);
		_dataStorage.Clear();
	}
}
