using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PitchState : State
{
	[SerializeField]
	private float maxTimer;

	private float timer;

	private Pitching _pitching = null;

	private void Start()
	{
		_pitching = GetComponent<Pitching>();
	}
	public override bool IsStateOn()
	{
		if (GameManager.Instance.State == BattingState.Idle)
			return true;
		else
			return false;
	}

	public override void StateOn()
	{
		if(maxTimer > timer)
		{
			timer += Time.deltaTime;
		}
		else
		{
			_pitching.Shoot();
			timer = 0;
		}
	}
}
