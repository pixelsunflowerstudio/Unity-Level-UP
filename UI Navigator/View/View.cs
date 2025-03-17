using UnityEngine;
using Cysharp.Threading.Tasks;
using Game.Defines;

namespace Game.UI
{
	[RequireComponent(typeof(RectTransform), typeof(CanvasGroup))]
	public class View : MonoBehaviour
	{
		public virtual string Identifier { get; set; }

		public ViewAnimationType Animation;

		[HideInInspector] public CanvasGroup CanvasGroup;
		public Transform Container;

		protected virtual void Awake()
		{
			CanvasGroup = GetComponent<CanvasGroup>();
		}

		public virtual UniTask Initialize()
		{
			return UniTask.CompletedTask;
		}

		public virtual async void Show()
		{
			CanvasGroup.interactable = true;

			await OnShow();

			gameObject.SetActive(true);
		}

		public virtual async void Hide()
		{
			CanvasGroup.interactable = false;

			await OnHide();

			gameObject.SetActive(false);
		}

		protected virtual UniTask OnShow()
		{
			return UniTask.CompletedTask;
		}

		protected virtual UniTask OnHide()
		{
			return UniTask.CompletedTask;
		}

		public virtual void OnFinishedShow()
		{

		}
	}
}