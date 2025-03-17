using Cysharp.Threading.Tasks;
using DG.Tweening;
using Game.Utilities;
using System;
using UnityEngine;

namespace Game.UI
{
	public class DiamondGridTransition : ViewTransition
	{
		[SerializeField] private UIDiamondGridGenerator DiamondGridGenerator;

		[SerializeField] private WipeDirection Direction;

		[Header("Customizations"), Space]
		[SerializeField] private float TransitionDuration;
		[SerializeField] private float DiamondScaleTime;
		[SerializeField] private Ease EaseType;

		private Transform[,] _grid;
		private Transform _container;

		private int rows = 0;
		private int columns = 0;

		protected override void OnAwake()
		{
			_grid = DiamondGridGenerator.GenerateGrid();
			_container = DiamondGridGenerator.Container;
			_container.gameObject.SetActive(false);

			rows = _grid.GetLength(0);
			columns = _grid.GetLength(1);

			HideGrid();
		}

		private void OnDisable()
		{
			ResetGrid();
		}

		protected override void OnPlayTransition()
		{
			gameObject.SetActive(true);

			switch (Direction)
			{
				case WipeDirection.WipeUp:
					WipeGridUp();
					break;
				case WipeDirection.WipeDownFromBottom:
					WipeGridOutFromBottom();
					break;
			}
		}

		// Show Grid from the Bottom -> Top
		private async void WipeGridUp()
		{
			HideGrid();
			_canvasGroup.alpha = 1;
			_container.gameObject.SetActive(true);

			float delay = TransitionDuration / rows;

			for (int i = 0; i < rows; i++)
			{
				for (int j = 0; j < columns; j++)
				{
					var go = _grid[i, j];
					go.localScale = Vector3.zero;
					go.gameObject.SetActive(true);

					go.DOScale(1f, 0.1f).SetEase(EaseType).SetUpdate(true);
				}
				await Delay(delay);
			}

			OnTransition();

			// Fade out Grid
			await FadeOutGrid();

			EndTransition();
		}

		// Hide Grid from the Top -> Bottom
		public async void WipeGridOutFromBottom()
		{
			ShowGrid();
			_canvasGroup.alpha = 0;
			_container.gameObject.SetActive(true);

			// Fade In Grid
			await FadeInGrid();

			OnTransition();

			float delay = TransitionDuration / rows;

			for (int i = 0; i < rows; i++)
			{
				for (int j = 0; j < columns; j++)
				{
					var go = _grid[i, j];

					go.transform.DOScale(0f, 0.1f).SetEase(EaseType).SetUpdate(true);
				}
				await Delay(delay);
			}

			EndTransition();
		}

		private void ShowGrid()
		{
			foreach (var gridElement in _grid)
			{
				gridElement.gameObject.SetActive(true);
			}
		}

		private void HideGrid()
		{
			foreach (var gridElement in _grid)
			{
				gridElement.gameObject.SetActive(false);
			}
		}

		private async UniTask FadeInGrid()
		{
			var tween = DOVirtual.Float(0, 1, 0.5f, (a) => _canvasGroup.alpha = a);
			await UniTask.WaitUntil(() => !tween.IsActive());
		}

		private async UniTask FadeOutGrid()
		{
			var tween = DOVirtual.Float(1, 0, 0.5f, (a) => _canvasGroup.alpha = a);
			await UniTask.WaitUntil(() => !tween.IsActive());
		}

		private void ResetGrid()
		{
			foreach (var gridElement in _grid)
			{
				gridElement.gameObject.SetActive(false);
				gridElement.localScale = Vector3.one;
			}

			_canvasGroup.alpha = 1f;
		}

		private async UniTask Delay(float duration)
		{
			await UniTask.Delay(TimeSpan.FromSeconds(duration), ignoreTimeScale: true);
		}

		private enum WipeDirection
		{
			WipeUp,
			WipeDownFromBottom,
		}
	}
}