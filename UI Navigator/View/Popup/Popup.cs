using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Game.UI
{
	public class Popup : View
	{
		[SerializeField] private string _identifier;
		public override string Identifier
		{
			get
			{
				if (string.IsNullOrEmpty(_identifier))
				{
					_identifier = gameObject.name.Replace("(Clone)", string.Empty);
				}

				return _identifier;
			}

			set => _identifier = value;
		}

		// Popups are destroyed when hidden.
		public override async void Hide()
		{
			await OnHide();

			Destroy(gameObject);
		}
	}
}