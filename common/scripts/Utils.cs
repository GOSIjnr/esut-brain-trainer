using Godot;
using System;
using System.Collections.Generic;

namespace GOSIjnr;

public static class Utils
{
	// Merges the template dictionary into the base dictionary.
	// For each key in the template, if the base dictionary contains a value,
	// that value is used; otherwise, the template value is used.
	public static IDictionary<TKey, TValue> MergeDictionaries<TKey, TValue>(IDictionary<TKey, TValue> baseDictionary, IDictionary<TKey, TValue> templateDictionary)
	{
		Dictionary<TKey, TValue> mergedDictionary = [];

		foreach (var templatePair in templateDictionary)
		{
			if (baseDictionary.TryGetValue(templatePair.Key, out TValue baseValue))
			{
				mergedDictionary[templatePair.Key] = baseValue;
			}
			else
			{
				mergedDictionary[templatePair.Key] = templatePair.Value;
			}
		}

		return mergedDictionary;
	}

	// Provides extension methods for safely retrieving values from a dictionary.
	// Specifically designed to handle cases where the key may not exist or the value
	// may not be of the expected type, allowing for a default value to be returned when necessary.
	public static T GetValueOrDefault<[MustBeVariant] TKey, [MustBeVariant] TValue, [MustBeVariant] T>(this IDictionary<TKey, TValue> dict, TKey key, Func<T> defaultValueProvider, Predicate<T> validator = null)
	{
		T storedValue = defaultValueProvider();

		if (dict.TryGetValue(key, out TValue value))
		{
			if (value is T directValue)
			{
				storedValue = directValue;
			}
			else if (value is Variant variant)
			{
				try
				{
					storedValue = variant.As<T>();
				}
				catch
				{
					Logger.Log($"Failed to convert variant to type {typeof(T)}: {variant}", Logger.LogLevel.Warning);
				}
			}

			if (validator == null || validator(storedValue))
			{
				return storedValue;
			}
			else if (validator(storedValue) == false)
			{
				Logger.Log($"Invalid value range for key '{key}' with default value: {defaultValueProvider()}", Logger.LogLevel.Warning);
			}
		}

		Logger.Log($"Failed to retrieve value for key '{key}' with default value: {defaultValueProvider()}", Logger.LogLevel.Warning);
		return storedValue;
	}

	// Loads all resources from the specified folder path using the Godot file system.
	public static Resource[] GetResourcesFromFolder(string folderPath)
	{
		List<Resource> loadedResources = [];

		if (string.IsNullOrEmpty(folderPath)) return [.. loadedResources];

		string normalizedFolderPath = folderPath.EndsWith("/", StringComparison.OrdinalIgnoreCase) ? folderPath : folderPath + "/";

		var fileNames = ResourceLoader.ListDirectory(normalizedFolderPath);

		if (fileNames.Length == 0)
		{
			Logger.Log($"No resources found in folder: {normalizedFolderPath}");
		}

		foreach (var fileName in fileNames)
		{
			Resource resource = ResourceLoader.Load(normalizedFolderPath + fileName);

			if (resource != null) loadedResources.Add(resource);
		}

		return [.. loadedResources];
	}

	// Loads all resources of the specified type T from the given folder path.
	// Only resources that can be successfully cast to T are returned.
	public static T[] GetResourcesFromFolder<T>(string folderPath) where T : Resource
	{
		List<T> loadedResources = [];

		if (string.IsNullOrEmpty(folderPath)) return [.. loadedResources];

		string trimmedFolderPath = folderPath.Trim();
		string normalizedFolderPath = trimmedFolderPath.EndsWith("/", StringComparison.OrdinalIgnoreCase) ? trimmedFolderPath : trimmedFolderPath + "/";

		var fileNames = ResourceLoader.ListDirectory(normalizedFolderPath);

		if (fileNames.Length == 0)
		{
			Logger.Log($"No resources found in folder: {normalizedFolderPath}");
		}

		foreach (var fileName in fileNames)
		{
			try
			{
				T loadedResource = ResourceLoader.Load<T>(normalizedFolderPath + fileName);

				if (loadedResource != null)
				{
					loadedResources.Add(loadedResource);
				}
			}
			catch (Exception ex)
			{
				Logger.Log($"Failed to load resource {normalizedFolderPath + fileName}: {ex.Message}", Logger.LogLevel.Error);
			}
		}

		return [.. loadedResources];
	}
}
