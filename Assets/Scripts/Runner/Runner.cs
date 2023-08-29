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
	public Vector3 RunObjectVec => runObjectVec;

	bool isRun = false;
	public bool IsRun => isRun;

	private bool _isOut = false;
	public bool IsOut => _isOut;

	Base _base;

	private Vector3 _startPosition = Vector3.zero;
	public Vector3 StartPos => _startPosition;
	private void Awake()
	{
		animator = GetComponent<Animator>();
		_navMeshAgent = GetComponent<NavMeshAgent>();
		GameManager.Instance.onStateChange += Run;
		GameManager.Instance.onChangeGameMode += DestroyRunner;
	}

	private void Start()
	{
		_navMeshAgent = GetComponent<NavMeshAgent>();
		Debug.Log(_navMeshAgent);
	}

	private void OnDestroy()
	{
		if(GameManager.Instance != null)
		{
			GameManager.Instance.onStateChange -= Run;
			GameManager.Instance.onChangeGameMode -= DestroyRunner;
		}
	}

	private void Update()
	{
		if(Vector3.Distance(this.transform.position, _navMeshAgent.destination) < 1)
		{
			isRun = false;
		}
		animator.SetBool("Run", isRun);
	}

	private void Run(BattingState state)
	{
		if (state == BattingState.Batting)
		{
			RunBase();
		}
	}

	public void RunBase()
	{
		if (runIndex != 0)
		{
			Base currentbase = BaseControll.Instance.BaseReturn(runIndex);
			currentbase.ExitBase();
		}
		_startPosition = this.transform.position;
		_base = BaseControll.Instance.BaseReturn(runIndex++);
		_base.BaseRunRunner(this);
		runObjectVec = _base.enemybasePos.transform.position;
		isRun = true;
		_navMeshAgent.SetDestination(runObjectVec);
	}

	public void HomeRun()
	{
		RunnerManager.Instance.onBaseOn?.Invoke(this);
		RunBase();
	}

	public void Out()
	{
		RunnerManager.Instance.RemoveRunner(this);
		_isOut = true;
		_base.ComeRunFailed();
		Destroy(this.gameObject);
	}

	private void DestroyRunner(Mode mode)
	{
		RunnerManager.Instance.RemoveRunner(this);
		Destroy(this.gameObject);
	}

	public void DestroyRunner()
	{
		RunnerManager.Instance.RemoveRunner(this);
		Destroy(this.gameObject);
	}
}
