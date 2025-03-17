using UnityEngine;
using DG.Tweening;
using Game.Defines;

namespace Game.UI
{
	[CreateAssetMenu(menuName = "View Animation/Pop Zoom", fileName = nameof(ViewAnimationType.PopZoom))]
	public class PopZoom : ViewAnimation
	{
		public float Duration = 0.25f;
		public float StartAlpha = 0.5f;
		public float StartScale = 0.9f;
		public float TargetScale = 1f;
		public float Overshoot = 4f;

		public override Sequence PlayShowAnimation(View view)
		{
			_animation = DOTween.Sequence();

			view.Container.localScale = Vector3.one * StartScale;
			view.CanvasGroup.alpha = StartAlpha;

			_animation.Append(view.Container.transform.DOScale(TargetScale, Duration).SetEase(Ease.OutBack, Overshoot)).SetUpdate(true);
			_animation.Join(DOVirtual.Float(0, 1, Duration, (a) => view.CanvasGroup.alpha = a));

			return _animation;
		}

		public override Sequence PlayHideAnimation(View view)
		{
			_animation = DOTween.Sequence();

			view.Container.localScale = Vector3.one;
			view.CanvasGroup.alpha = 1f;

			_animation.Append(view.Container.transform.DOScale(TargetScale + 0.05f, Duration).SetEase(Ease.OutBack, Overshoot)).SetUpdate(true);
			_animation.Join(DOVirtual.Float(1, 0, Duration, (a) => view.CanvasGroup.alpha = a));

			return _animation;
		}

		public override Sequence PlayTransition(View from, View to)
		{
			_animation = DOTween.Sequence();
			from.Hide();
			to.Show();
			Debug.LogWarning($"Transition of type: {ViewAnimationType.PopZoom} has not been implemented");

			return _animation;
		}
	}
}