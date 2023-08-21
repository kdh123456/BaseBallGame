using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class FuturePathState : State
{
	private GameObject futurePathObject;

	private NavMeshAgent _navMeshAgent;

	private void Start()
	{
		futurePathObject = new GameObject();
		_navMeshAgent = GetComponent<NavMeshAgent>();
	}
	public override void StateOn()
	{
		Vector3 vec = GameManager.Instance.BallObject.transform.position;

		Vector3 defenVec = this.transform.position;
		Vector3 defenExpectMinuVec = new Vector3(vec.x * Mathf.Abs(defenVec.z), 1, defenVec.z);

		if (Vector3.Distance(defenVec, defenExpectMinuVec) < 10)
		{
			futurePathObject.transform.position = defenExpectMinuVec;
			_navMeshAgent.SetDestination(futurePathObject.transform.position);
		}
	}

	public override bool IsStateOn()
	{
		Vector3 vec = GameManager.Instance.BallObject.transform.position;

		Vector3 defenVec = this.transform.position;
		Vector3 defenExpectMinuVec = new Vector3(vec.x * Mathf.Abs(defenVec.z), 1, defenVec.z);

		if (Vector3.Distance(defenVec, defenExpectMinuVec) < 10)
		{
			return true;
		}

		return false;
	}
}
