using UnityEngine;

public class PlayerEntity : Entity
{
	private void Awake()
	{
		StateMachine = new PlayerStateMachine(this);
	}
}
