using System;
using UnityEngine;

public class ExamplePlayerManager : MonoBehavior
{
	[SerializeField] private PlayerEntity _player;

	private void OnEnable()
	{
		_player.StateMachine.SetState(PlayerStateType.Start, new PlayerStateDataStart()
		{
			StartSpeed = 10f
		});
	}

	private void OnDisable()
	{
		_player.StateMachine.SetState(PlayerStateType.Dead, new PlayerStateDataDead()
		{
			DeathPosition = _player.transform.position
		});
	}

	private void OnDestroy()
	{
		_player.StateMachine.Exit();
	}
}
