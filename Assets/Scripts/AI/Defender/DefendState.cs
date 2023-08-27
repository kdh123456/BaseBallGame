using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class DefendState : State
{
	protected NavMeshAgent _agent;
	protected Animator _animator;

	protected Defend _defend;

	protected virtual void Start()
	{
		_agent = GetComponent<NavMeshAgent>();
		_animator = GetComponent<Animator>();
		_defend = GetComponent<Defend>();
	}
}

