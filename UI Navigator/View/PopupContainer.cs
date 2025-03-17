using System.Linq;
using Cysharp.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Game.UI
{
	public class PopupContainer : MonoBehaviour, IViewContainer<Popup>
	{
		public static PopupContainer Main;

		private readonly List<Popup> _popupList = new List<Popup>();
		public Popup Current => _popupList.Count == 0 ? null : _popupList[^1];
		public bool TransitionBlock => false;
		public bool HasPopup => Current != null;

		[SerializeField] private Image Overlay;

		private void Awake()
		{
			Main = this;
			Overlay.gameObject.SetActive(_popupList.Any());
		}

		public void ShowPopup(string viewName, bool playAnimation, bool stack = true, bool enableOverlay = true)
		{
			ViewNavigator.ShowView(this, viewName, playAnimation, stack, enableOverlay: enableOverlay);
		}

		public async void ShowView(ViewNavigationConfig<Popup> option)
		{
			Overlay.gameObject.SetActive(option.EnableOverlay);

			Popup popup = Instantiate(option.View, parent: transform);
			await popup.Initialize();

			popup.Show();
			if (option.Stack)
				_popupList.Add(popup);

			foreach (var current in _popupList)
			{
				current.CanvasGroup.interactable = (current == popup);
			}

			if (option.PlayAnimation)
			{
				await ViewAnimationController.PlayShowAnimation(popup.Animation, popup);
			}
		}

		public async void HideCurrentView(bool playAnimation)
		{
			Popup popup = Current;

			await HidePopup(popup, playAnimation);
		}

		public async UniTask HidePopup(string identifier, bool playAnimation)
		{
			var popup = GetPopup<Popup>(identifier);
			await HidePopup(popup, playAnimation);
		}

		private async UniTask HidePopup(Popup popup, bool playAnimation)
		{
			Overlay.gameObject.SetActive(_popupList.Count > 1);

			if (popup == null) return;

			popup.CanvasGroup.interactable = false;

			if (_popupList.Contains(popup))
			{
				_popupList.Remove(popup);
			}

			if (_popupList.Count > 0)
			{
				var topMostPopup = _popupList[^1];
				foreach (var current in _popupList)
				{
					current.CanvasGroup.interactable = (current == topMostPopup);
				}
			}

			if (playAnimation)
			{
				await ViewAnimationController.PlayHideAnimation(popup.Animation, popup);
			}
			else
			{
				popup.Hide();
			}

			Overlay.gameObject.SetActive(_popupList.Any());
		}

		public async UniTask HideAll(bool playAnimation)
		{
			Overlay.gameObject.SetActive(false);

			if (playAnimation)
			{
				foreach (Popup popup in _popupList)
				{
					await ViewAnimationController.PlayHideAnimation(popup.Animation, popup);
				}
			}
			else
			{
				foreach (Popup popup in _popupList)
				{
					popup.Hide();
				}
				_popupList.Clear();
			}

			Overlay.gameObject.SetActive(_popupList.Any());
		}

		public TPopup GetPopup<TPopup>(string identifier) where TPopup : Popup
		{
			foreach (Popup popup in _popupList)
			{
				if (popup.Identifier == identifier)
					return popup as TPopup;
			}
			return null;
		}
	}
}