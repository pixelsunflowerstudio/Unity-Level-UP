using Cysharp.Threading.Tasks;
using Game.Defines;
using System.Collections.Generic;
using UnityEngine;

namespace Game.UI
{
	public class ScreenContainer : MonoBehaviour, IViewContainer<Screen>
	{
		public static ScreenContainer Main;

		private readonly Dictionary<string, Screen> _viewDictionaryCache = new Dictionary<string, Screen>();
		private readonly Stack<Screen> _viewStack = new Stack<Screen>();
		public Screen Current => _viewStack.Count == 0 ? null : _viewStack.Peek();
		public bool TransitionBlock => true;

		private void Awake()
		{
			Main = this;
		}

		private async void ShowView(Screen from, Screen to, bool playAnimation, bool playTransition = false, ViewTransitionType viewTransitionType = ViewTransitionType.None)
		{
			if (playAnimation)
			{
				await ViewAnimationController.PlayTransition(from, to);
			}
			else if (playTransition && from != null && viewTransitionType != ViewTransitionType.None)
			{
				await ViewAnimationController.PlayTransition(from, to, viewTransitionType);
			}
			else
			{
				if (from != null) from.Hide();
				to.CanvasGroup.alpha = 1;
				to.Show();
				to.OnFinishedShow();
			}
		}

		public void ShowView(ViewNavigationConfig<Screen> option)
		{
			Screen nextScreen = GetScreen(option.ViewIdentifier);
			Screen previousScreen = _viewStack.Count == 0 ? null : _viewStack.Peek();

			if (option.Stack) _viewStack.Push(nextScreen);

			ShowView(previousScreen, nextScreen, option.PlayAnimation, playTransition: true, option.TransitionType);
		}

		public void HideCurrentView(bool playAnimation)
		{
			if (_viewStack.Count <= 1)
			{
				Debug.LogWarning("Cannot hide any more screens");
				return;
			}

			Screen previousScreen = Current;
			_viewStack.Pop();
			Screen nextScreen = _viewStack.Peek();

			ShowView(previousScreen, nextScreen, playAnimation);
		}

		public UniTask HideAll(bool playAnimation)
		{
			Debug.LogWarning("Cannot perform Hide All For Screens");
			return UniTask.CompletedTask;
		}

		public Screen GetScreen(string viewKey)
		{
			Screen screen;

			if (!_viewDictionaryCache.ContainsKey(viewKey))
			{
				var prefab = ViewResourceCollection.GetView<Screen>(viewKey);
				screen = Instantiate(prefab, transform);
				_viewDictionaryCache.Add(viewKey, screen);

				screen.Initialize();

				screen.gameObject.SetActive(false);
			}
			else
			{
				screen = _viewDictionaryCache[viewKey];
			}
			return screen;
		}
	}
}