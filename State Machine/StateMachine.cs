using System.Collections.Generic;
using System;
using UnityEngine;

public interface IStateMachine
{
	// Set new State
	public void SetState(Enum state, StateData stateData = null);

	// Called in Monobehavior Update. Can also add FixedUpdate, or LateUpdate if needed.
	public void Update();

	// Call when you need the State Machine to Stop
	public void Exit();

	// Validate current state
	public bool IsState(Enum state);
}

public abstract class StateMachine<TStateMachine, TEntity> : IStateMachine
{
	private TEntity _owner;
	private State<TStateMachine> _previousState;
	private State<TStateMachine> _currentState;
	private List<State<TStateMachine>> _states = new List<State<TStateMachine>>();

	public TEntity Owner => _owner;

	public Action<Enum> OnStateChanged;

	public StateMachine(TEntity owner)
	{
		_owner = owner;
	}

	protected void AddState(State<TStateMachine> state)
	{
		_states.Add(state);
	}

	public void SetState(Enum state, StateData stateData = null)
	{
		_previousState = _currentState;
		if (_previousState != null) _previousState.Exit();

		_currentState = _states[Convert.ToInt32(state)];
		_currentState.Enter(stateData);

		OnStateChanged?.Invoke(state);
	}

	public void Update()
	{
		if (_currentState != null)
			_currentState.Update();
	}

	public void Exit()
	{
		if (_currentState != null)
		{
			_currentState.Exit();
			_currentState = null;
		}
	}

	public bool IsState(Enum state)
	{
		return _currentState == _states[Convert.ToInt32(state)];
	}
}