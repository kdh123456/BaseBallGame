using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Selector : State
{
	[SerializeField]
	private State[] _states;

	public override void StateOn()
	{
		foreach (var state in _states)
		{
			if (state.IsStateOn())
			{
				state.StateOn();
				return;
			}
		}
	}

	public override bool IsStateOn()
	{
		foreach(var state in _states) 
		{
			if (state.IsStateOn())
				return true;
		}
		return false;
	}
}
