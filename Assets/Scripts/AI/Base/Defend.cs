using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Defend : MonoBehaviour
{
	//여기서 처음에 selecter을 쓴다.

	//그 후에 Sequence를 사용해서 state를 계속 실행 시켜준다ㅏ.

	[SerializeField]
	protected State _state;

	protected bool _isDefend = false;

	protected Ball _haveBall = null;
	public Ball HaveBall => _haveBall;

	protected Vector3 _firstPosition = Vector3.zero;
	protected Quaternion _quaternion = Quaternion.identity;
	protected NavMeshAgent _agent = null;

	private void Awake()
	{
		_firstPosition = transform.position;
		_quaternion = transform.rotation;
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
	protected virtual void ResetPosition(BattingState state)
	{
		if(state == BattingState.Idle)
		{
			_agent.isStopped = true;
			Destroy(_haveBall);
			_agent.enabled = false;
			this.transform.position = _firstPosition;
			this.transform.rotation = _quaternion;
			_agent.enabled = true;
		}
	}
}
