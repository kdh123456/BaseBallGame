using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.AI;

public class Defend : MonoBehaviour
{
	//여기서 처음에 selecter을 쓴다.

	//그 후에 Sequence를 사용해서 state를 계속 실행 시켜준다ㅏ.

	[SerializeField]
	private State _state;

	private bool _isDefend = false;

	private Ball _haveBall = null;
	public Ball HaveBall => _haveBall;

	private Vector3 _firstPosition = Vector3.zero;
	private NavMeshAgent _agent = null;

	private void Awake()
	{
		_firstPosition = transform.position;
		_agent = GetComponent<NavMeshAgent>();
	}

	private void Start()
	{
		GameManager.Instance.onStateChange += DefendOn;
		GameManager.Instance.onStateChange += ResetPosition;
	}

	protected virtual void Update()
	{   
		if (_isDefend)
		{
			_isDefend = _state.IsStateOn();

			_state.StateOn();
		}
	}

	private void DefendOn(BattingState state)
	{
		if (state == BattingState.Batting)
		{
			_isDefend = true;
		}
	}
	public void BallSet(Ball ball)
	{
		_haveBall = ball;
	}
	private void ResetPosition(BattingState state)
	{
		if(state == BattingState.Idle)
		{
			_agent.isStopped = true;
			_agent.enabled = false;
			Destroy(_haveBall);
			this.transform.position = _firstPosition;
			_agent.enabled = true;
		}
	}
}
