using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IfDefndingState : DefendState
{
	private Defend _defend;

	protected override void Start()
	{
		base.Start();
		_defend = GetComponent<Defend>();
	}
	public override void StateOn()
	{
		_agent.isStopped = true;
	}

	public override bool IsStateOn()
	{
		if (GameManager.Instance.State == BattingState.Batting)
			return true;
		else
			return false;
	}
}
