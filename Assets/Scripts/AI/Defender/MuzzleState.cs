using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MuzzleState : DefendState
{
	private LayerMask ballLayerMask;

	private Transform trans;

	public bool _isMuzzle = false;
	protected override void Start()
	{
		base.Start();
		ballLayerMask = LayerMask.GetMask("Ball");

		Transform[] obj = transform.GetComponentsInChildren<Transform>();
		foreach (Transform obj2 in obj)
		{
			if (obj2.name == "mixamorig:RightHand")
			{
				trans = obj2;
			}
		}

		_defend = GetComponent<Defend>();

		GameManager.Instance.onStateChange += ResetMuzzleState;
	}
	public override bool IsStateOn()
	{
		Collider disCol = DisColliderCheck(ballLayerMask, 1f);

		Ball disball = disCol?.GetComponent<Ball>();

		if (disball == null)
			return false;

		if (_isMuzzle)
			return false;

		return true;
	}

	private Collider DisColliderCheck(LayerMask mask, float radius)
	{
		Collider[] col = Physics.OverlapSphere(this.transform.position, radius, mask);
		Collider disCol = null;
		foreach (Collider collider in col)
		{
			if (disCol == null)
			{
				disCol = collider;
				continue;
			}

			float currentDis = Vector3.Distance(this.transform.position, disCol.transform.position);
			float defDis = Vector3.Distance(this.transform.position, collider.transform.position);

			if (currentDis > defDis)
				disCol = collider;
		}

		return disCol;
	}

	public override void StateOn()
	{
		Collider disCol = DisColliderCheck(ballLayerMask, 1f);
		if (disCol == null)
			return;

		Ball disball = disCol.GetComponent<Ball>();

		if (disball.Flying)
		{
			RunnerManager.Instance.OutRunner();
			GameManager.Instance.AddOut();
			GameManager.Instance.WaitReset();
		}

		_animator.SetBool("Catch", true);
		_defend.BallSet(disball);

		disball.Muzzle(trans);
		_animator.SetBool("Chase", false);
		_isMuzzle = true;
	}

	public void ResetMuzzleState(BattingState state)
	{
		if(state == BattingState.Idle)
		{
			if (_agent.isActiveAndEnabled)
				_agent.isStopped = true;
			_animator.SetBool("Catch", false);
			_animator.SetBool("Chase", false);
			_isMuzzle = false;
		}
	}
}
