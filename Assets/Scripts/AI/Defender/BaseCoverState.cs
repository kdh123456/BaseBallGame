using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class BaseCoverState : DefendState
{
	public bool baseCoverOn = false;
	public Base _coverBase = null;

	private bool _baseCover = false;
	public Base CoverBase
	{
		get
		{
			if(_baseCover)
				return _coverBase;
			return null;
		}
	}

	protected override void Start()
	{
		base.Start();
		GameManager.Instance.onStateChange += ResetBaseCoverState;
	}

	public override void StateOn()
	{
		if (baseCoverOn)
		{
			_agent.isStopped = false ;
			_coverBase.BaseCovering();
			_agent.SetDestination(_coverBase.playerbasePos.transform.position);
			_animator.SetBool("Chase", true);

			if(_agent.remainingDistance < 1)
				_animator.SetBool("Chase", false);
		}
	}

	public void BaseIn()
	{
		baseCoverOn = false;
		_baseCover = true;
	}

	public void BaseOut() => _baseCover = false;

	public override bool IsStateOn()
	{
		if (baseCoverOn)
			return true;

		return false;
	}

	private void ResetBaseCoverState(BattingState state)
	{
		if(state == BattingState.Idle)
		{
			_agent.isStopped = true;
			baseCoverOn = false;
			_coverBase = null;
			_baseCover = false;
			_animator.SetBool("Chase", false);
		}
	}
}
