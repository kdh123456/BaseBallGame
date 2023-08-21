using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.AI;

public enum DefenderStateEnum
{
	Idle = 0,
	BaseCover,
	BallChase,
	FuturePathChase
}

public class Defender : MonoBehaviour
{
	public Dictionary<DefenderStateEnum, float> weightOfState = new Dictionary<DefenderStateEnum, float>();

	private Transform trans;

	private GameObject target;

	private Animator _animator;
	private NavMeshAgent _agent;

	private Ball _haveBall;
	private Base _thisBase = null;

	public bool IsBase => _thisBase != null;

	//private LayerMask baseLayerMask;
	private LayerMask ballLayerMask;

	private bool _isBallMuzzle = false;

	private Vector3 _originVec;
	private Vector3 _futurePathVec;

	private GameObject baseCoverObj;

	private DefenderStateEnum _state;

	public Ball HaveBall => _haveBall;

	private void Start()
	{
		_animator = GetComponent<Animator>();
		_agent = GetComponent<NavMeshAgent>();

		//baseLayerMask = LayerMask.GetMask("Base");
		ballLayerMask = LayerMask.GetMask("Ball");

		Transform[] obj = transform.GetComponentsInChildren<Transform>();

		foreach (Transform obj2 in obj)
		{
			if (obj2.name == "mixamorig:RightHand")
			{
				trans = obj2;
			}
		}

		foreach (DefenderStateEnum state in Enum.GetValues(typeof(DefenderStateEnum)))
		{
			weightOfState.Add(state, 0);
		}

		_originVec = this.transform.position;

		GameManager.Instance.onStateChange += ResetDefender;
	}

	private void Update()
	{
		if (GameManager.Instance.State != BattingState.Batting)
			return;
		if (_agent.remainingDistance < 2)
		{
			_animator.SetBool("Chase", false);
		}

		if (!_isBallMuzzle)
			BallCheck();

		float upValue = 9999;
		foreach (KeyValuePair<DefenderStateEnum, float> value in weightOfState)
		{
			if (value.Value == 0)
				continue;

			if (upValue > value.Value)
			{
				upValue = value.Value;
				_state = value.Key;
			}
		}

		switch (_state)
		{
			case DefenderStateEnum.BaseCover:
				TargetChase(baseCoverObj);
				baseCoverObj.GetComponent<Base>().BaseCovering();
				break;
			case DefenderStateEnum.BallChase:
				TargetChase(GameManager.Instance.BallObject);
				break;
			case DefenderStateEnum.FuturePathChase:
				FuturePath();
				break;
		}
		//BaseCoverCheck();
	}

	//private void BaseCoverCheck()
	//{
	//	Collider disCol = DisColliderCheck(baseLayerMask, 10);

	//	Base disBase = disCol?.GetComponent<Base>();
	//	if (disBase == null)
	//		return;
	//	if (!disBase.IsBaseCover)
	//	{
	//		disBase.BaseCovering();
	//		target = disBase.gameObject;
	//	}
	//}
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

	//여기서 base랑 path이동 그리고 ball이동을 사용한다.
	public void TargetChase(GameObject obj)
	{
		if (_agent.remainingDistance > 1)
		{
			_animator.SetBool("Chase", true);
		}
		_agent.SetDestination(obj.transform.position);
	}
	public void TargetChaseStop()
	{
		if (target != null)
			_agent.SetDestination(target.transform.position);
		else
		{
			_agent.isStopped = true;
			_animator.SetBool("Chase", false);
		}

	}


	public void FuturePathSetting(Vector3 vec)
	{
		_futurePathVec = vec;
	}
	public void FuturePath()
	{
		GameObject obj = new GameObject("targetPos");
		obj.transform.position = _futurePathVec;
		TargetChase(obj);
	}

	public void BaseCover(GameObject obj)
	{
		baseCoverObj = obj;
	}

	public void BaseIn(Base hbase)
	{
		_thisBase = hbase;
	}
	public void BaseOut()
	{
		_thisBase = null;
	}

	private void BallCheck()
	{
		Collider disCol = DisColliderCheck(ballLayerMask, 1f);

		Ball disball = disCol?.GetComponent<Ball>();
		if (disball == null)
			return;

		_isBallMuzzle = true;
		_animator.SetBool("Catch", true);
		_haveBall = disball;

		_animator.SetBool("Throw", true);

		GameManager.Instance.ChangeState(BattingState.Defending);
		disball.Muzzle(trans);
	}

	public void ThrowCheck()
	{
		_animator.SetBool("Catch", false);

		if (!BaseControll.Instance.ThrowBaseHave(_thisBase))
		{
			_animator.SetBool("Throw", false);
			GameManager.Instance.ChangeState(BattingState.Idle);
		}
	}
	public void Throw()
	{
		Vector3 vec = BaseControll.Instance.ThrowBaseReturn().transform.position - trans.position;
		_haveBall.transform.parent = null;
		_haveBall.transform.position = trans.transform.position + Vector3.right;
		_haveBall.Shoot(vec.normalized, Vector3.left, 100f, 5f);
	}

	public void DefenderStateChange(DefenderStateEnum defender, GameObject obj)
	{
		_state = defender;
		TargetChase(obj);
	}

	private void ResetDefender(BattingState state)
	{
		if (state == BattingState.Idle)
		{
			TargetChaseStop();
			foreach (DefenderStateEnum states in Enum.GetValues(typeof(DefenderStateEnum)))
			{
				weightOfState[states] = 0;
			}
			_agent.enabled = false;

			this.transform.position = _originVec;
			if (this.gameObject.name == "2BaseDefender")
				Debug.Log(this.transform.position);
			Debug.Break();
		}
	}

	private void OnDrawGizmos()
	{
		Color color = Color.white;
		//color.a = 0.25f;
		//Gizmos.color = color;
		//Gizmos.DrawSphere(this.transform.position, 10);

		color = Color.red;
		color.a = 0.25f;
		Gizmos.color = color;
		Gizmos.DrawSphere(this.transform.position, 1.5f);
	}
}
