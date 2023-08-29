using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class BallHaveBaseCoverState : DefendState
{
	private BaseCoverState _state;
	protected override void Start()
	{
		base.Start();
		_state = GetComponent<BaseCoverState>();
	}
	public override bool IsStateOn()
	{
		if (_state.baseCoverOn && _defend.HaveBall && _state.CoverBase.Running)
			return true;

		return false;
	}

	public override void StateOn()
	{

		_state.StateOn();
	}
}
