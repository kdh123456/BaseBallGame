using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;

public class Catcher : MonoBehaviour
{
	[SerializeField]
	private StrikeZone zone;

	[SerializeField]
	private GameObject _catchVec;

	private Animator _animator;

	private bool isCatch = false;

	private void Start()
	{
		_animator = GetComponent<Animator>();

		GameManager.Instance.onStateChange += CatchStart;
	}

	public void Check(Collider other)
	{
		if (other.CompareTag("Ball") && GameManager.Instance.State != BattingState.Batting)
		{
			if (zone.isSktrike)
				Strike();
			else if (!zone.isSktrike && GameManager.Instance.State == BattingState.Bat)
				Strike();
			else if (!zone.isSktrike)
				Ball();
		}
	}

	private void Strike()
	{
		GameManager.Instance.AddStrike();
		zone.isSktrike = false;
		CatchEnd();
	}

	private void Ball()
	{
		GameManager.Instance.AddBall();
		CatchEnd();
	}

	private void CatchStart(BattingState state)
	{
		if(state == BattingState.Pitch)
			isCatch = true;
	}

	private void CatchEnd() => isCatch = false;

	private void OnAnimatorIK(int layerIndex)
	{
		if(isCatch)
		{
			_animator.SetIKPosition(AvatarIKGoal.LeftHand , _catchVec.transform.position);
			_animator.SetIKPositionWeight(AvatarIKGoal.LeftHand, 1);
			//_animator.SetIKRotation(AvatarIKGoal.LeftHand, _catchVec.transform.rotation);
			//_animator.SetIKRotationWeight(AvatarIKGoal.LeftHand, 1);
		}
	}
}
