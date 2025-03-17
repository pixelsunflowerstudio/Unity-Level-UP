using System.Collections.Generic;
using UnityEngine;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using Game.Defines;

namespace Game.UI
{
	public class ViewAnimationController
	{
		public const string ViewAnimationResource = "ScriptableObject/ViewAnimation/";
		public const string ViewTransitionResource = "Prefab/ViewTransition/";

		private static Dictionary<ViewAnimationType, ViewAnimation> _animationDictionary = new();
		private static Dictionary<ViewTransitionType, ViewTransition> _transitionDictionary = new();

		private static ViewAnimation GetAnimation(ViewAnimationType type)
		{
			if (!_animationDictionary.ContainsKey(type))
			{
				var handler = Resources.Load<ViewAnimation>(ViewAnimationResource + type);
				_animationDictionary.Add(type, handler);
			}
			return _animationDictionary[type];
		}

		private static ViewTransition GetTransition(ViewTransitionType type)
		{
			if (!_transitionDictionary.ContainsKey(type))
			{
				var transitionPrefab = Resources.Load<ViewTransition>(ViewTransitionResource + type);
				var transition = GameObject.Instantiate(transitionPrefab, TransitionContainer.Instance.transform);
				_transitionDictionary.Add(type, transition);
			}
			return _transitionDictionary[type];
		}

		public static async UniTask PlayTransition(View from, View to, ViewTransitionType viewTransitionType)
		{
			var transition = GetTransition(viewTransitionType);
			await transition.PlayTransition(from, to);
		}

		public static async UniTask PlayTransition(View from, View to)
		{
			if (from == null && to == null)
			{
				Debug.LogError("Views are null!");
				return;
			}
			else if (from == null)
			{
				await PlayShowAnimation(to.Animation, to);
				return;
			}
			else
			{
				await PlayHideAnimation(from.Animation, from);
				await PlayShowAnimation(to.Animation, to);
			}
		}

		public static async UniTask PlayShowAnimation(ViewAnimationType type, View target)
		{
			if (target == null)
			{
				Debug.LogError("View Target is null!");
				return;
			}
			target.Show();
			Sequence sequence = GetAnimation(type).PlayShowAnimation(target);
			await UniTask.WaitUntil(() => !sequence.IsActive());

			target.OnFinishedShow();
		}

		public static async UniTask PlayHideAnimation(ViewAnimationType type, View target)
		{
			if (target == null)
			{
				Debug.LogError("View Target is null!");
				return;
			}
			Sequence sequence = GetAnimation(type).PlayHideAnimation(target);
			await UniTask.WaitUntil(() => !sequence.IsActive());
			target.Hide();
		}
	}
}