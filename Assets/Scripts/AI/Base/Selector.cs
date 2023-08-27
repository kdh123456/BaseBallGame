using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Selector : State
{
	[SerializeField]
	protected State[] _states;

	public State _currentState;

	public override void StateOn()
	{
		foreach (var state in _states)
		{
			if (state.IsStateOn())
			{
				_currentState = state;
				state.StateOn();
				return;
			}
		}
	}

	public override bool IsStateOn()
	{
		foreach (var state in _states)
		{
			if (state.IsStateOn())
				return true;
		}
		return false;
	}
}
