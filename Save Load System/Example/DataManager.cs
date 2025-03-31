using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DataManager
{
	private static List<IDataStorage> _storages = new();

	public static void Init()
	{
		_storages = new()
		{
			new ProgressionDataStorage(),
			new UserDataStorage(),
		};

		Load();
	}

	public static IDataStorage GetStorage(StorageType storageType)
	{
		foreach (IDataStorage storage in _storages)
		{
			if (storage.StorageType == storageType)
				return storage;
		}

		Debug.LogError($"Storage Does not exist: {storageType}");
		return null;
	}

	public static T GetData<T>(StorageType storageType) where T : IStorageModel
	{
		foreach (var storage in _storages)
		{
			if (storage.StorageType == storageType)
				return (T)storage.GetData();
		}

		Debug.LogError($"Storage Does not exist: {storageType}");
		return default;
	}

	public static void AutoSaveAll()
	{
		foreach (var storage in _storages)
		{
			if (!storage.ManualSave)
				storage.Save();
		}
	}

	public static void Load()
	{
		foreach (IDataStorage storage in _storages)
		{
			storage.Load();
		}
	}
}