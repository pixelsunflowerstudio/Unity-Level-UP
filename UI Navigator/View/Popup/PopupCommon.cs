using Cysharp.Threading.Tasks;
using Game.Runtime;
using Game.Utilities;
using System;
using TMPro;
using UnityEngine;

namespace Game.UI
{
	public class PopupCommon : Popup
	{
		[SerializeField] private TextMeshProUGUI TextContent;
		[SerializeField] private CooldownButton ButtonConfirm;
		[SerializeField] private CooldownButton ButtonCancel;
		[SerializeField] private CooldownButton ButtonOk;

		private Action _confirmCallback;
		private Action _cancelCallback;

		protected override UniTask OnShow()
		{
			PopupCommonData data = EventManager.GetData<PopupCommonData>(EventName.UI.OnShowPopupCommon);
			SetData(data);

			return base.OnShow();
		}

		private void OnEnable()
		{
			ButtonConfirm.onClick.AddListener(OnConfirm);
			ButtonOk.onClick.AddListener(OnConfirm);
			ButtonCancel.onClick.AddListener(OnCancel);
		}

		private void OnDisable()
		{
			ButtonConfirm.onClick.RemoveListener(OnConfirm);
			ButtonOk.onClick.RemoveListener(OnConfirm);
			ButtonCancel.onClick.RemoveListener(OnCancel);
		}

		public void SetData(PopupCommonData data)
		{
			SetData(data.IsConfirmation, data.Content, data.ConfirmCallback, data.CancelCallback);
		}

		public void SetData(bool isConfirmation, string content, Action confirmCallback = null, Action cancelCallback = null)
		{
			ButtonConfirm.gameObject.SetActive(isConfirmation);
			ButtonCancel.gameObject.SetActive(isConfirmation);
			ButtonOk.gameObject.SetActive(!isConfirmation);

			TextContent.text = LocalizationManager.GetString(content);

			_confirmCallback = confirmCallback;
			_cancelCallback = cancelCallback;
		}

		private void OnConfirm()
		{
			_confirmCallback?.Invoke();
			ViewNavigator.HideCurrentView(PopupContainer.Main, true);
		}

		private void OnCancel()
		{
			_cancelCallback?.Invoke();
			ViewNavigator.HideCurrentView(PopupContainer.Main, true);
		}
	}

	public struct PopupCommonData
	{
		public bool IsConfirmation;
		public string Content;
		public Action ConfirmCallback;
		public Action CancelCallback;

		public PopupCommonData(bool isConfirmation, string content, Action confirmCallback = null, Action cancelCallback = null)
		{
			IsConfirmation = isConfirmation;
			Content = content;
			ConfirmCallback = confirmCallback;
			CancelCallback = cancelCallback;
		}
	}
}