using System;
using UnityEngine;

public abstract class Entity : MonoBehavior
{
	public IStateMachine StateMachine;

	protected virtual void Update()
	{
		if (StateMachine != null)
		{
			StateMachine.Update();
		}
	}
}
