using Game.Runtime;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

/// <summary>
/// Attach this script to any Text object that needs to display localized text
/// </summary>
public class LocalizedText : MonoBehaviour
{
	/// <summary>
	/// Input the key of the needed text
	/// </summary>
	[SerializeField] private string LocalizeKey;

	private TextMeshProUGUI _text;

	private void Awake()
	{
		_text = GetComponent<TextMeshProUGUI>();
	}

	private void OnEnable()
	{
		LocalizationManager.OnLanguageChanged += OnLanguageChanged;

		Refresh();
	}

	private void OnDisable()
	{
		LocalizationManager.OnLanguageChanged -= OnLanguageChanged;
	}

	private void OnLanguageChanged()
	{
		Refresh();
	}

	private void Refresh()
	{
		_text.SetText(LocalizationManager.GetString(LocalizeKey));
	}
}