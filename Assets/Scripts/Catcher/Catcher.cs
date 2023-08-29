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
	private bool noHit = false;

	private void Start()
	{
		_animator = GetComponent<Animator>();

		GameManager.Instance.onStateChange += CatchStart;
		DefendController.Instance.onBating += Hit;
		DefendController.Instance.onBating += CatchEnd;
	}

	public void Check(Collider other)
	{
		if (other.CompareTag("Ball"))
		{

			if (GameManager.Instance.State == BattingState.Bat)
			{
				Strike();
				return;
			}

			if (zone.isSktrike)
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
		noHit = false;
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

	private void Hit() => noHit = true;

	private void OnAnimatorIK(int layerIndex)
	{
		if(isCatch)
		{
			_animator.SetIKPosition(AvatarIKGoal.LeftHand , _catchVec.transform.position);
			_animator.SetIKPositionWeight(AvatarIKGoal.LeftHand, 1);
		}
	}
}
