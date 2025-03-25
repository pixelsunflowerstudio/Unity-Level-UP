using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(LocalizationCollection))]
public class LocalizationCollectionEditor : Editor
{
	public override void OnInspectorGUI()
	{
		base.OnInspectorGUI();

		if (GUILayout.Button("Populate Data From CSV"))
		{
			var localizationCollection = (LocalizationCollection)target;
			localizationCollection.PopulateDataFromCSV();
		}
	}
}