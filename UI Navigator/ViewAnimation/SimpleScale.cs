using UnityEngine;
using DG.Tweening;
using Game.Defines;

namespace Game.UI
{
	[CreateAssetMenu(menuName = "View Animation/Simple Scale", fileName = nameof(ViewAnimationType.SimpleScale))]
	public class SimpleScale : ViewAnimation
	{
		public float Duration = 0.5f;
		public Ease ShowEaseType;
		public Ease HideEaseType;

		public override Sequence PlayShowAnimation(View view)
		{
			_animation = DOTween.Sequence();

			view.Container.transform.localScale = Vector3.zero;
			view.CanvasGroup.alpha = 0f;

			_animation.Append(view.Container.transform.DOScale(1, Duration).SetEase(ShowEaseType)).SetUpdate(true);
			_animation.Join(DOVirtual.Float(0, 1, Duration * 0.25f, (a) => view.CanvasGroup.alpha = a));

			return _animation;
		}

		public override Sequence PlayHideAnimation(View view)
		{
			_animation = DOTween.Sequence();

			view.Container.transform.localScale = Vector3.one;
			view.CanvasGroup.alpha = 1f;

			_animation.Append(view.Container.transform.DOScale(0, Duration).SetEase(HideEaseType)).SetUpdate(true);

			return _animation;
		}

		public override Sequence PlayTransition(View from, View to)
		{
			_animation = DOTween.Sequence();
			from.Hide();
			to.Show();
			Debug.LogWarning($"Transition of type: {ViewAnimationType.SimpleScale} has not been implemented");

			return _animation;
		}
	}
}