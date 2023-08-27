using System.Collections;
using System.Collections.Generic;
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
}
