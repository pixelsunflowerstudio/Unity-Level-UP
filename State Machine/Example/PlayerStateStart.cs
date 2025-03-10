using UnityEngine;

// State Data for this State
public class PlayerStateDataStart : StateData
{
	public float StartSpeed;
}

// Populate this class with State-specific logc
public class PlayerStateStart : State<PlayerStateMachine>
{
	private float _startSpeed;

	public PlayerStateStart(PlayerStateMachine context) : base(context)
	{
	}

	public override void Enter(StateData stateData = null)
	{
		if (stateData != null)
		{
			_startSpeed = stateData.StartSpeed;
		}
	}

	public override void Exit()
	{
	}

	public override void Update()
	{
	}
}