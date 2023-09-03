using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class ThrowState : DefendState
{
	private Transform trans;

	private MuzzleState _muzzleState;
	private BaseCoverState _coverState;

	protected override void Start()
	{
		base.Start();
		Transform[] obj = transform.GetComponentsInChildren<Transform>();
		foreach (Transform obj2 in obj)
		{
			if (obj2.name == "mixamorig:RightHand")
			{
				trans = obj2;
			}
		}

		_defend = GetComponent<Defend>();
		_muzzleState = GetComponent<MuzzleState>();
		_coverState = GetComponent<BaseCoverState>();

		GameManager.Instance.onStateChange += ResetThrowState;
	}

	public override void StateOn()
	{
		base.StateOn();
		ThrowCheck();
	}
	public override bool IsStateOn()
	{
		if (_defend.HaveBall != null)
			return true;
		else if (_defend.HaveBall == null)
			return false;
		else
			return false;
	}

	public void ThrowCheck()
	{
		if (BaseControll.Instance.ThrowBaseHave(_coverState.CoverBase) && _defend.HaveBall != null)
		{
			_animator.SetBool("Throw", true);
			Vector3 vec = BaseControll.Instance.ThrowBaseReturn(this.transform.position,_coverState.CoverBase).transform.position - trans.position;			
			this.transform.LookAt(vec);
			return;
		}
		else if(!BaseControll.Instance.ThrowBaseHave(_coverState.CoverBase) || _defend.HaveBall == null)
		{
			_animator.SetBool("Throw", false) ;
		}

	}

	public void Throw()
	{
		if (!BaseControll.Instance.ThrowBaseHave(_coverState.CoverBase) || _defend.HaveBall == null)
			return;
		_agent.enabled = false;
		Vector3 vec = BaseControll.Instance.ThrowBaseReturn(this.transform.position,_coverState.CoverBase).transform.position - trans.position;
		this.transform.LookAt(vec);
		_defend.HaveBall.transform.parent = null;
		_defend.HaveBall.transform.position = trans.transform.position + Vector3.right;
		_defend.HaveBall.DefendThrow(BaseControll.Instance.ThrowBaseReturn(this.transform.position,_coverState.CoverBase).transform.position);
		_muzzleState._isMuzzle = false;
		_defend.BallSet(null);
		_agent.enabled = true;
	}

	private void ResetThrowState(BattingState state)
	{
		if(state == BattingState.Idle)
		{
			_agent.isStopped = true;
			_animator.SetBool("Catch", false);
			_animator.SetBool("Throw", false);
		}
	}
}
