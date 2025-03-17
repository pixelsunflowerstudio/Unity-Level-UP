using UnityEngine;
using Cysharp.Threading.Tasks;
using Game.Defines;

namespace Game.UI
{
	public static class ViewNavigator
	{
		private static bool IsInTransition = false;

		public static ViewNavigationConfig<T> GetViewNavigateOption<T>(string viewName, bool playAnimation, bool playTransition, ViewTransitionType viewTransitionType, bool stack, bool enableOverlay) where T : View
		{
			return new ViewNavigationConfig<T>(viewName, playAnimation, playTransition, viewTransitionType, stack, enableOverlay);
		}

		public static void ShowView<T>(IViewContainer<T> viewContainer, string viewName, bool playAnimation, bool playTransition = false, ViewTransitionType viewTransitionType = ViewTransitionType.None, bool stack = true, bool enableOverlay = true) where T : View
		{
			ViewNavigationConfig<T> config = GetViewNavigateOption<T>(viewName, playAnimation, playTransition, viewTransitionType, stack, enableOverlay);

			ShowView(viewContainer, config);
		}

		private static void ShowView<T>(IViewContainer<T> viewContainer, ViewNavigationConfig<T> config) where T : View
		{
			if (viewContainer == null)
			{
				Debug.LogError("No view container found!");
				return;
			}

			if (config.View == null || config.View is not T)
			{
				Debug.LogError("View is not compatible!");
				return;
			}

			if (viewContainer.Current != null && viewContainer.Current.Identifier == config.ViewIdentifier)
			{
				Debug.LogWarning($"View Existed: {config.ViewIdentifier}");
				return;
			}

			if (IsInTransition && viewContainer.TransitionBlock)
			{
				Debug.LogWarning("ViewContainer is in Transition", viewContainer.Current);
				return;
			}

			IsInTransition = true;

			viewContainer.ShowView(config);

			IsInTransition = false;
		}

		public static void HideCurrentView<T>(IViewContainer<T> viewContainer, bool playAnimation) where T : View
		{
			if (viewContainer == null)
			{
				Debug.LogError("No view container found!");
				return;
			}

			if (viewContainer.Current == null)
			{
				Debug.LogWarning("Current View is null! Nothing to Hide");
				return;
			}

			if (IsInTransition && viewContainer.TransitionBlock)
			{
				Debug.LogWarning("ViewContainer is in Transition", viewContainer.Current.gameObject);
				return;
			}

			IsInTransition = true;

			viewContainer.HideCurrentView(playAnimation);

			IsInTransition = false;
		}

		public async static void HideAll<T>(IViewContainer<T> viewContainer, bool playAnimation) where T : View
		{
			if (viewContainer == null)
			{
				Debug.LogError("No view container found!");
				return;
			}

			if (viewContainer.Current == null)
			{
				Debug.LogWarning("Current View is null! Nothing to Hide");
				return;
			}

			if (IsInTransition && viewContainer.TransitionBlock)
			{
				Debug.LogWarning("ViewContainer is in Transition", viewContainer.Current.gameObject);
				return;
			}

			IsInTransition = true;

			await viewContainer.HideAll(playAnimation);

			IsInTransition = false;
		}
	}

	public class ViewNavigationConfig<T> where T : View
	{
		public T View { get; }
		public bool PlayAnimation { get; }
		public bool PlayTransition { get; }
		public ViewTransitionType TransitionType { get; }
		public bool Stack { get; }
		public bool EnableOverlay { get; }
		public string ViewIdentifier { get; }


		public ViewNavigationConfig(string viewKey, bool playAnimation, bool playTransition, ViewTransitionType viewTransitionType, bool stack, bool enableOverlay)
		{
			View = ViewResourceCollection.GetView<T>(viewKey);
			PlayAnimation = playAnimation;
			PlayTransition = playTransition;
			TransitionType = viewTransitionType;
			Stack = stack;
			ViewIdentifier = viewKey;
			EnableOverlay = enableOverlay;
		}
	}

	public interface IViewContainer<T> where T : View
	{
		public T Current { get; }
		public bool TransitionBlock { get; }

		public void ShowView(ViewNavigationConfig<T> option);

		public void HideCurrentView(bool playAnimation);

		public UniTask HideAll(bool playAnimation);
	}
}