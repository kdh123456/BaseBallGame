using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class BallChaseState : State
{
	public bool baseCoverOn = false;
	private Ball _coverBall;

	private NavMeshAgent _agent;

	private void Start()
	{
		_agent = GetComponent<NavMeshAgent>();
		GameManager.Instance.onStateChange += BallSetting;
	}

	private void BallSetting(BattingState state)
	{
		if(state == BattingState.Defending)
		{
			_coverBall = GameManager.Instance.BallObject.GetComponent<Ball>();
		}
	}
	public override void StateOn()
	{

	}

	public override bool IsStateOn()
	{
		return false;
		//if (_coverBall == null)
		//	return false;
		//
		//if(_coverBall == )
	}
}
