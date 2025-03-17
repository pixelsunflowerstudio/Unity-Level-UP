using UnityEngine;

namespace Game.UI
{
	public class Screen : View
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
	}
}
