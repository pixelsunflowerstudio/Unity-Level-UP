using UnityEngine;

// State Data for this State
public class PlayerStateDataDead : StateData
{
	public Vector3 DeathPosition;
}

// Populate this class with State-specific logc
public class PlayerStateDead : State<PlayerStateMachine>
{
	private Vector3 _deathPosition;

	public PlayerStateDead(PlayerStateMachine context) : base(context)
	{
	}

	public override void Enter(StateData stateData = null)
	{
		if (stateData != null)
		{
			_deathPosition = stateData.deathPosition;
		}
	}

	public override void Exit()
	{
	}

	public override void Update()
	{
	}
}