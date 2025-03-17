using UnityEngine;
using DG.Tweening;

namespace Game.UI
{
	public abstract class ViewAnimation : ScriptableObject
	{
		protected Sequence _animation;
		public abstract Sequence PlayTransition(View from, View to);
		public abstract Sequence PlayShowAnimation(View view);
		public abstract Sequence PlayHideAnimation(View view);
	}
}
