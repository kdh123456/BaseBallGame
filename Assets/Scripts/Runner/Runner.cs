using System.Collections;
using System.Collections.Generic;
using Unity.Services.Analytics.Internal;
using UnityEngine;
using UnityEngine.AI;
using static UnityEngine.GraphicsBuffer;

public class Runner : MonoBehaviour
{
	private int runIndex = 0;
	private int thisIndex = 0;

	private Animator animator;

	private NavMeshAgent _navMeshAgent;

	private Vector3 runObjectVec;

	bool isRun = false;

	private void Awake()
	{
		animator = GetComponent<Animator>();
		_navMeshAgent = GetComponent<NavMeshAgent>();
	}

	private void Start()
	{
		GameManager.Instance.onStateChange += (state) =>
		{
			if (state == BattingState.Batting)
				RunBase();
		};
	}

	private void Update()
	{
		if(Vector3.Distance(this.transform.position, _navMeshAgent.destination) < 1)
		{
			isRun = false;
		}
		animator.SetBool("Run", isRun);
	}

	public void RunBase()
	{
		if (runIndex != 0)
		{
			Base currentbase = BaseControll.Instance.BaseReturn(runIndex);
			currentbase.ExitBase();
		}
		Base nextbase = BaseControll.Instance.BaseReturn(runIndex++);
		runObjectVec = nextbase.transform.position;
		isRun = true;
		_navMeshAgent.SetDestination(runObjectVec);
	}
}
