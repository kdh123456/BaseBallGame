using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class FuturePathState : DefendState
{
	private bool _isFuturePath = false;
	private Vector3 _futurePath;

	protected override void Start()
	{
		base.Start();
		GameManager.Instance.onStateChange += ResetFuturePathState;
	}

	public void FutureSet(Vector3 futurePath)
	{
		_isFuturePath = true;
		this._futurePath = futurePath;
	}
	public override void StateOn()
	{
		if(_isFuturePath)
		{
			_agent.isStopped = false;
			_agent.SetDestination(_futurePath);
			_animator.SetBool("Chase", true);
		}
		else if(_agent.remainingDistance < 1f)
		{
			_isFuturePath = false;
			_animator.SetBool("Chase", false);
		}
	}

	public override bool IsStateOn()
	{
		if (_isFuturePath && !_defend.HaveBall)
			return true;
		else
			return false;
	}

	private void ResetFuturePathState(BattingState state)
	{
		if(state == BattingState.Idle)
		{
			_agent.isStopped = true;
			_isFuturePath = false;
			_animator.SetBool("Chase", false);
		}
	}
}
