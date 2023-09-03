using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using UnityEngine;

public class Pitcher : Defend
{
	[SerializeField]
	private State _pitchState;

	bool _isPitch = false;
	protected override void Update()
	{
		_isPitch = _pitchState.IsStateOn();
		if(_isPitch && GameManager.Instance.gameMode == Mode.BatMode)
		{
			_pitchState.StateOn();
		}
		base.Update();
	}

	protected override void ResetPosition(BattingState state)
	{
		if (state == BattingState.Idle)
		{
			_agent.isStopped = true;
			Destroy(_haveBall);
			//_agent.enabled = false;
			this.transform.position = _firstPosition;
			this.transform.rotation = _quaternion;
			//_agent.enabled = true;
		}
	}
}
