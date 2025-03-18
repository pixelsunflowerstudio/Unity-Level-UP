using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(CSVCollectionManager))]
public class CSVCollectionManagerEditor : Editor
{
	public override void OnInspectorGUI()
	{
		base.OnInspectorGUI();

		if (GUILayout.Button("Fetch CSV"))
		{
			var collectionManager = (CSVCollectionManager)target;
			_ = collectionManager.FetchCSV();
		}
	}
}
