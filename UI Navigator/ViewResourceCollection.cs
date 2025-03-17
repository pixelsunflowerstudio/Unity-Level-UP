using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
using Game.Defines;

namespace Game.UI
{
	[CreateAssetMenu(fileName = "ViewResourceCollection", menuName = "Resources/ViewResource")]
	public class ViewResourceCollection : ScriptableObject
	{
		public const string AddressPrefix = "Prefab/View/";

		private static Dictionary<string, string> ViewAddressDictionary;

		private static Dictionary<string, View> ViewCollection;

		public List<ViewLoadingConfig> LoadConfig;

		public static T GetView<T>(string viewKey) where T : View
		{
			if (!ViewAddressDictionary.ContainsKey(viewKey))
			{
				Debug.LogError($"View key: <color=red> {viewKey} </color> does not exist!");
				return null;
			}

			if (ViewCollection.ContainsKey(viewKey))
			{
				return ViewCollection[viewKey] as T;
			}

			var view = Resources.Load<View>(ViewAddressDictionary[viewKey]);
			if (view == null)
			{
				Debug.LogError($"No view found. key: {viewKey}");
				return null;
			}

			ViewCollection.Add(viewKey, view);
			return view as T;
		}

		public void LoadViewConfig()
		{
			for (int i = 0; i < LoadConfig.Count; i++)
			{
				if (LoadConfig[i].Preload)
				{
					View view = Resources.Load<View>(ViewAddressDictionary[LoadConfig[i].ViewName]);
					if (view == null)
					{
						Debug.LogError($"No view found. key: {LoadConfig[i].ViewName}");
					}
					else
					{
						ViewCollection.Add(LoadConfig[i].ViewName, view);
						Debug.Log($"Preloaded: {LoadConfig[i].ViewName}");
					}
				}
			}
		}

		private void OnEnable()
		{
			Initialize();
		}

		private void Initialize()
		{
			if (ViewAddressDictionary == null) ViewAddressDictionary = new Dictionary<string, string>();
			if (LoadConfig == null) LoadConfig = new List<ViewLoadingConfig>();
			ViewCollection = new Dictionary<string, View>();

			FieldInfo[] fields = typeof(ViewKey).GetFields(BindingFlags.Public | BindingFlags.Static);

			List<string> keys = new List<string>();

			foreach (var item in fields)
			{
				if (item.FieldType != typeof(string)) continue;

				string key = (string)item.GetValue(null);
				keys.Add(key);
				string address = AddressPrefix + key;

				if (!ViewAddressDictionary.ContainsKey(key))
				{
					ViewAddressDictionary.Add(key, address);
				}

				bool existingConfig = false;
				for (int i = 0; i < LoadConfig.Count; i++)
				{
					if (LoadConfig[i].ViewName == key)
					{
						existingConfig = true;
						break;
					}
				}

				if (!existingConfig)
				{
					LoadConfig.Add(new ViewLoadingConfig(viewName: key));
				}
			}
			LoadConfig.RemoveAll(x => !keys.Contains(x.ViewName));
		}
	}

	[System.Serializable]
	public class ViewLoadingConfig
	{
		public string ViewName;
		public bool Preload;

		public ViewLoadingConfig(string viewName, bool preload = true)
		{
			ViewName = viewName;
			Preload = preload;
		}
	}
}