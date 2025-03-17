using Cysharp.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.UI
{
	public class ViewTransition : MonoBehaviour
	{
		protected Animator _animator;
		protected CanvasGroup _canvasGroup;

		protected View _from;
		protected View _to;

		private void Awake()
		{
			_animator = GetComponent<Animator>();
			_canvasGroup = GetComponent<CanvasGroup>();

			OnAwake();
		}

		protected virtual void OnAwake()
		{
		}

		public UniTask PlayTransition(View from, View to)
		{
			_from = from;
			_to = to;

			OnPlayTransition();

			return UniTask.CompletedTask;
		}

		protected virtual void OnPlayTransition()
		{
			gameObject.SetActive(true);

			if (_animator != null)
				_animator.Play("Transition");
		}

		public void StartTransition()
		{

		}

		public void OnTransition()
		{
			if (_from != null) _from.Hide();
			if (_from != null) _to.Show();
		}

		public void EndTransition()
		{
			_to.OnFinishedShow();
			gameObject.SetActive(false);
		}
	}
}