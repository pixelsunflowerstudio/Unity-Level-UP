using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LocalizationManager
{
	// Change this to where ever the LocalizationCollection Scriptable Object is
	private static const string LocalizationCollectionAssetAddress = "LocalizationCollection";

	public static event Action OnLanguageChanged;

	private static Dictionary<Language, Dictionary<string, string>> _localizationDictionary = new()
		{
			{Language.EN, new Dictionary<string, string>() },
			{Language.VN, new Dictionary<string, string>() },
		};

	private static Language _currentLanguage;
	public static Language CurrentLanguage => _currentLanguage;

	public static void Init()
	{
		var localizationData = Resources.Load<LocalizationCollection>(LocalizationCollectionAssetAddress);

		var dataTable = localizationData.DataTable;
		foreach (var data in dataTable)
		{
			_localizationDictionary[Language.EN].TryAdd(data.Key, data.EN);
			_localizationDictionary[Language.VN].TryAdd(data.Key, data.VN);
		}

		ChangeLanguage(Language.EN);
	}

	public static void ChangeLanguage(Language language)
	{
		if (!_localizationDictionary.ContainsKey(language))
		{
			Debug.LogError($"Cannot change to language: {language}");
			return;
		}

		_currentLanguage = language;
		OnLanguageChanged?.Invoke();
	}

	public static string GetString(string key)
	{
		if (_localizationDictionary[_currentLanguage].TryGetValue(key, out string localizedString))
		{
			return localizedString;
		}
		Debug.LogError($"Missing Localize Key for: {key}");
		return "Missing Localization";
	}

	public static string GetString(string key, Language targetLanguage)
	{
		if (_localizationDictionary[targetLanguage].TryGetValue(key, out string localizedString))
		{
			return localizedString;
		}
		Debug.LogError($"Missing Localize Key for: {key}");
		return "Missing Localization";
	}
}