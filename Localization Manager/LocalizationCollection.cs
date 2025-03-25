using System;
using System.IO;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

/// <summary>
/// This class represent each data row within the CSV.
/// Each variable name should be EXACTLY the same on the first row of the CSV file. The order can be changed, but for clarity, it's best to keep the same order.
/// </summary>
[Serializable]
public class LocalizationData
{
	public string Key;
	public string EN;
	public string VN;
}

[CreateAssetMenu(fileName = "LocalizationCollection", menuName = "Scriptable Object/Localization Collection")]
public class LocalizationCollection : ScriptableObject
{
	// This path points to the CSV Collection Manager Scriptable Object. 
	private const string CSVCollectionPath = "Assets/ScriptableObject/CSVCollectionManager/CSVCollectionManager.asset";

	public List<LocalizationData> DataTable;

	/// <summary>
	/// This function will read the data from the CSV file, and populate the DataTable. This should be done in editor time to update the Localization Data
	/// each time you update the CSV data.
	/// </summary>
	public void PopulateDataFromCSV()
	{
		CSVCollectionManager collectionManager = AssetDatabase.LoadAssetAtPath<CSVCollectionManager>(CSVCollectionPath);

		TextAsset textAsset = new TextAsset();
		if (collectionManager != null)
		{
			textAsset = collectionManager.GetCollection(nameof(LocalizationData));
			if (textAsset == null)
			{
				Debug.LogError($"Text Asset collection with name: {nameof(LocalizationData)} doesn't exist.");
				return;
			}
		}

		DataTable.Clear();
		DataTable.AddRange(CSVImporter.Parse<LocalizationData>(textAsset.ToString()));

		AssetDatabase.SaveAssets();
	}
}