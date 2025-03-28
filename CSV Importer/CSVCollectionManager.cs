using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.Networking;
using Cysharp.Threading.Tasks;

#region Summary
// This works with Google Sheets. The online Google Sheets file will need to be public. And each sheet within the file will be one collection.

// This is the script that will download the CSV files and save them, and create a collection of all downloaded files.
// Any class that needs to gain access to a CSV file, can call the GetCollection function.

// This script requires the UniTask package. But if you do not have the package, you can modify the function to use a coroutine instead. UniTask is just more useful.
#endregion

#region Download URL Template Guide
// Sheet public link:
// For example, the public link to the Google Sheet file is this: https://docs.google.com/spreadsheets/d/1FSDBXegmaxGpJgsbUeMQlN6EuyjR8YggLFEX68y_IV4/edit?usp=sharing
// REMOVE the ending: "edit?usp=sharing"
// Then use as the GsDatabasePath below
// Correct example: https://docs.google.com/spreadsheets/d/1FSDBXegmaxGpJgsbUeMQlN6EuyjR8YggLFEX68y_IV4/

// Sheet ID
// In the browser address: https://docs.google.com/spreadsheets/d/1FSDBXegmaxGpJgsbUeMQlN6EuyjR8YggLFEX68y_IV4/edit?gid=1169038705#gid=1169038705
// The sheet ID changes depending on what sheet it is. in this case: 1169038705
// The sheet ID can be found behind the: "gid"
#endregion

[CreateAssetMenu(fileName = "CSVCollectionManager", menuName = "Scriptable Object/CSV Tools/CSV Collection Manager")]
public class CSVCollectionManager : ScriptableObject
{
	// CSV files will be downloaded to this path. Change this if needed
	private const string DownloadPathTemplate = "Assets/CSV/{0}.csv";
	// Download URL Template for exporting Google Sheet as a CSV file. {0} will be the public link to the Sheet file. {1} is the sheet ID found at the end of the link of {0}
	private const string DownloadUrlTemplate = "{0}export?format=csv&gid={1}";

	public string GsDatabasePath;
	public List<CSVDownloadData> DownloadData;

	public List<TextAsset> Collection;

	private string GetDownloadPath(string sheetId)
	{
		return string.Format(DownloadUrlTemplate, GsDatabasePath, sheetId);
	}

	/// <summary>
	/// Fetch a collection by name
	/// </summary>
	/// <param name="collectionName">Collection name should always be the class name of what you're parsing the CSV into.</param>
	/// <returns></returns>
	public TextAsset GetCollection(string collectionName)
	{
		foreach (var item in Collection)
		{
			if (item.name == collectionName)
				return item;
		}
		return null;
	}

	/// <summary>
	/// This function downloads the CSV files from Google Sheets, then add Text Assets to the Collection list.
	/// </summary>
	/// <returns></returns>
	public async UniTask FetchCSV()
	{
		Collection.Clear();

		foreach (var data in DownloadData)
		{
			string url = GetDownloadPath(data.SheetId);
			UnityWebRequest webRequest = UnityWebRequest.Get(url);
			await webRequest.SendWebRequest();

			await UniTask.WaitUntil(() => webRequest.isDone);

			string csv = webRequest.downloadHandler.text;
			string savePath = string.Format(DownloadPathTemplate, data.ClassName);

			File.WriteAllText(savePath, csv);
			AssetDatabase.Refresh();

			var textAsset = AssetDatabase.LoadAssetAtPath<TextAsset>(savePath);
			if (textAsset != null)
			{
				Collection.Add(textAsset);
			}
		}

		EditorUtility.DisplayDialog(title: "Download CSV",
		   message: "Download CSV finish", "OK");
	}
}

[Serializable]
public class CSVDownloadData
{
	public string ClassName;
	public string SheetId;
}
