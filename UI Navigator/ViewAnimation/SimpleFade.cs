using UnityEngine;
using DG.Tweening;
using Game.Defines;

namespace Game.UI
{
	[CreateAssetMenu(menuName = "View Animation/Simple Fade", fileName = nameof(ViewAnimationType.SimpleFade))]
	public class SimpleFade : ViewAnimation
	{
		public override Sequence PlayTransition(View from, View to)
		{
			_animation = DOTween.Sequence();

			from.CanvasGroup.alpha = 1f;
			to.CanvasGroup.alpha = 0f;

			Tween fadeOut = DOVirtual.Float(1, 0, 0.5f, a => from.CanvasGroup.alpha = a).OnComplete(() =>
			{
				from.Hide();
				to.Show();
			});

			Tween fadeIn = DOVirtual.Float(0, 1, 0.5f, a => to.CanvasGroup.alpha = a);

			_animation.Append(fadeOut);
			_animation.Append(fadeIn);

			return _animation;
		}

		public override Sequence PlayShowAnimation(View view)
		{
			_animation = DOTween.Sequence();

			view.CanvasGroup.alpha = 0f;

			Tween fadeIn = DOVirtual.Float(0, 1, 0.5f, a => view.CanvasGroup.alpha = a);
			_animation.Append(fadeIn);

			return _animation;
		}

		public override Sequence PlayHideAnimation(View view)
		{
			_animation = DOTween.Sequence();

			view.CanvasGroup.alpha = 1f;
			Tween fadeOut = DOVirtual.Float(1, 0, 0.5f, a => view.CanvasGroup.alpha = a);
			_animation.Append(fadeOut);

			return _animation;
		}
	}
}