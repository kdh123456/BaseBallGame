using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sequence : State
{
	[SerializeField]
	private State[] _states;

	public override void StateOn()
	{
		foreach (var state in _states)
		{
			if (!state.IsStateOn())
				return;
			else
				state.StateOn();
		}
		
	}

	public override bool IsStateOn()
	{
		foreach (var state in _states)
		{
			if (!state.IsStateOn())
				return false;
		}
		return true;
	}
}
