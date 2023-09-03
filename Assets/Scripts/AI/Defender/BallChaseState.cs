using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class BallChaseState : DefendState
{
	public bool ballCoverOn = false;
	private Ball _coverBall;
	protected override void Start()
	{
		base.Start();
		GameManager.Instance.onStateChange += BallSetting;
		GameManager.Instance.onStateChange += ResetBallChaseState;
		_defend = GetComponent<Defend>();
	}

	private void BallSetting(BattingState state)
	{
		if (state == BattingState.Batting)
		{
			_coverBall = GameManager.Instance.BallObject.GetComponent<Ball>();
		}
	}
	public override void StateOn()
	{
		if (ballCoverOn)
		{
			_agent.isStopped = false;
			DefendController.Instance.BallCover();
			_agent.SetDestination(_coverBall.transform.position);
			_animator.SetBool("Chase", true);

			if (_agent.remainingDistance < 1)
				_animator.SetBool("Chase", false);

			if (_defend.HaveBall)
			{
				_animator.SetBool("Chase", false);
				_agent.isStopped = true;
				ballCoverOn = false;
				return;
			}

		}
	}

	public override bool IsStateOn()
	{
		if (ballCoverOn && !_defend.HaveBall)
		{
				return true;
		}
		else
			return false;
	}

	private void ResetBallChaseState(BattingState state)
	{
		if (state == BattingState.Idle)
		{
			if (_agent.isActiveAndEnabled)
				_agent.isStopped = true;
			_animator.SetBool("Chase", false);
			ballCoverOn = false;
			_coverBall = null;
		}
	}
}
