
// State Data is any information the current state needs to know
public class StateData
{

}

public interface IState
{
	// Called when the State is entered
	public void Enter(StateData stateData = null);

	// Called in Monobehavior Update. Can also add FixedUpdate, or LateUpdate if needed.
	public void Update();

	// Call when you need this State to Stop running its logic
	public void Exit();
}

public abstract class State<TStateMachine> : IState
{
	protected TStateMachine _context;

	public State(TStateMachine context)
	{
		_context = context;
	}

	public abstract void Enter(StateData stateData = null);
	public abstract void Exit();
	public abstract void Update();
	public abstract void FixedUpdate();
}