using Cysharp.Threading.Tasks;
using Game.Defines;
using Game.Runtime;
using Game.Runtime.Tutorials;
using Game.Utilities;
using System;
using UnityEngine;
using UnityEngine.UI;

namespace Game.UI
{
	public class ScreenMainMenu : Screen
	{
		[SerializeField] private CooldownButton ButtonUpgrade;
		[SerializeField] private CooldownButton ButtonSettings;
		[SerializeField] private CooldownButton ButtonShieldBuff;

		private ShieldBuffManager _shieldBuffManager;

		protected override UniTask OnShow()
		{
			EventManager.EmitEvent(EventName.UI.OnMenuTransition);

			return base.OnShow();
		}

		public override void OnFinishedShow()
		{
			if (TutorialManager.Instance.IsEnable)
				EventManager.EmitEvent(EventName.Tutorial.MainMenuShown);

			base.OnFinishedShow();
		}

		private void OnEnable()
		{
			if (_shieldBuffManager == null) _shieldBuffManager = ShieldBuffManager.Instance;

			MenuPlayerAnimationController.OnPlayAnimationFinished += OnPlay;

			ButtonUpgrade.onClick.AddListener(OnUpgrade);
			ButtonShieldBuff.onClick.AddListener(OnShieldBuff);
			ButtonSettings.onClick.AddListener(OnSettings);

			ButtonShieldBuff.gameObject.SetActive(PlayerProgressionManager.UnlockedShieldBuff);
			ButtonShieldBuff.interactable = !_shieldBuffManager.HasShieldBuff;
		}

		private void OnDisable()
		{
			MenuPlayerAnimationController.OnPlayAnimationFinished -= OnPlay;

			ButtonUpgrade.onClick.RemoveListener(OnUpgrade);
			ButtonShieldBuff.onClick.RemoveListener(OnShieldBuff);
			ButtonSettings.onClick.RemoveListener(OnSettings);
		}

		private void Update()
		{
			ButtonShieldBuff.interactable = !_shieldBuffManager.HasShieldBuff;
		}

		private void OnPlay()
		{
			EventManager.EmitEvent(EventName.Game.Load, LoadOperationType.LoadToGame);
			ViewNavigator.ShowView(ScreenContainer.Main, ViewKey.ScreenLoading, false, true, ViewTransitionType.WipeUp);
		}

		private void OnSettings()
		{
			ViewNavigator.ShowView(PopupContainer.Main, ViewKey.PopupSetting, true);
		}

		private void OnUpgrade()
		{
			ViewNavigator.ShowView(ScreenContainer.Main, ViewKey.ScreenUpgrade, true);
		}

		private void OnShieldBuff()
		{
			ViewNavigator.ShowView(PopupContainer.Main, ViewKey.PopupConfirmShieldBuff, true);
		}
	}
}