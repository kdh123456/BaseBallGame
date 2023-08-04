using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Defender : MonoBehaviour
{
	[SerializeField]
	private GameObject _baseObject;

	private GameObject target;

	private Animator _animator;
	private NavMeshAgent _agent;

	private bool _chase = false;

	private void Start()
	{
		_animator = GetComponent<Animator>();
		_agent = GetComponent<NavMeshAgent>();
	}

	private void Update()
	{
		if (Vector3.Distance(this.transform.position, _agent.destination) < 1)
		{
			_animator.SetBool("Chase", false);
		}
	}

	public void TargetChase(GameObject obj)
	{
		_animator.SetBool("Chase", true);
		_agent.SetDestination(obj.transform.position);
	}

	public void TargetChaseStop()
	{
		if (_baseObject != null)
			_agent.SetDestination(_baseObject.transform.position);
		else
			_agent.isStopped = true;
	}
}
