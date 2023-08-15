using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.AI;

public class Defender : MonoBehaviour
{
	private Transform trans;

	private GameObject target;

	private Animator _animator;
	private NavMeshAgent _agent;

	private Ball _haveBall;
	private Base _thisBase = null;

	private LayerMask baseLayerMask;
	private LayerMask ballLayerMask;

	private bool _isBallMuzzle = false;

	public Ball HaveBall => _haveBall;

	private Vector3 _originVec;

	private void Start()
	{
		_animator = GetComponent<Animator>();
		_agent = GetComponent<NavMeshAgent>();

		baseLayerMask = LayerMask.GetMask("Base");
		ballLayerMask = LayerMask.GetMask("Ball");

		Transform[] obj = transform.GetComponentsInChildren<Transform>();

		foreach (Transform obj2 in obj)
		{
			if (obj2.name == "mixamorig:RightHand")
			{
				trans = obj2;
			}
		}

		_originVec = this.transform.position;
	}

	private void Update()
	{
		if (_agent.remainingDistance < 2)
		{
			_animator.SetBool("Chase", false);
		}

		BaseCoverCheck();

		if (!_isBallMuzzle)
			BallCheck();

	}

	public void TargetChase(GameObject obj)
	{
		if (_agent.remainingDistance > 1)
		{
			_animator.SetBool("Chase", true);
		}
		_agent.SetDestination(obj.transform.position);
	}

	public void BaseIn(Base hbase)
	{
		_thisBase = hbase;
		Debug.Log("BaseIn");
	}

	public void BaseOut()
	{
		_thisBase = null;
		Debug.Log("BaseOut");
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

	private void BaseCoverCheck()
	{
		Collider disCol = DisColliderCheck(baseLayerMask, 10);

		Base disBase = disCol?.GetComponent<Base>();
		if (disBase == null)
			return;
		if (!disBase.IsBaseCover)
		{
			disBase.BaseCovering();
			target = disBase.gameObject;
		}
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

		Debug.Log("Muzzle");
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

	public void Throw()
	{
		Vector3 vec = BaseControll.Instance.ThrowBaseReturn().transform.position - trans.position;
		_haveBall.transform.parent = null;
		_haveBall.transform.position = trans.transform.position + Vector3.right;
		_haveBall.Shoot(vec.normalized, Vector3.left, 100f, 5f);
	}

	public void ThrowCheck()
	{
		_animator.SetBool("Catch", false);

		if (!BaseControll.Instance.ThrowBaseHave(_thisBase))
		{
			Debug.Log("ThrowFalse");
			_animator.SetBool("Throw", false);
			//GameManager.Instance.ChangeState(BattingState.Idle);
		}
	}

	private void OnDrawGizmos()
	{
		Color color = Color.white;
		color.a = 0.25f;
		Gizmos.color = color;
		Gizmos.DrawSphere(this.transform.position, 10);

		color = Color.red;
		color.a = 0.25f;
		Gizmos.color = color;
		Gizmos.DrawSphere(this.transform.position, 1.5f);
	}
}
