using Newtonsoft.Json;
using System;
using UnityEngine;

public interface IDataStorage
{
	public bool ManualSave { get; }
	public StorageType StorageType { get; }
	public ISaveData GetData();
	public void Save();
	public void Load();
}

public interface ISaveData
{
	public void InitData();
}

public abstract class DataStorage<T> : IDataStorage where T : ISaveData
{
	public bool ManualSave => _manualSave;
	protected abstract bool _manualSave { get; }

	public StorageType StorageType => _storageType;
	protected abstract StorageType _storageType { get; }

	protected T _data;

	public void Load()
	{
		string json = PlayerPrefs.GetString($"Storage{_storageType}");

		_data = JsonConvert.DeserializeObject<T>(json);

		if (_data == null)
		{
			InitStorage();
		}
	}

	public void Save()
	{
		string json = JsonConvert.SerializeObject(_data);

		PlayerPrefs.SetString($"Storage{_storageType}", json);

		Debug.Log("Saved: " + _storageType);
	}

	public ISaveData GetData()
	{
		return _data;
	}

	private void InitStorage()
	{
		_data = (T)Activator.CreateInstance(typeof(T));
		_data.InitData();

		Save();
	}
}