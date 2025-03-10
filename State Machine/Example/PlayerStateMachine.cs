public enum PlayerStateType
{
	Start,
	Dead,
}

public class PlayerStateMachine : StateMachine<PlayerStateMachine, Player>
{
	public PlayerStateMachine(Player owner) : base(owner)
	{
		AddState(new PlayerStateStart(this));
		AddState(new PlayerStateDead(this));
	}
}