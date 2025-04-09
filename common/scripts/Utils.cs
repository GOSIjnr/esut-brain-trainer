using Godot;
using System;
using System.Collections.Generic;

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
