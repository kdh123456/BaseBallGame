using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class Defend : MonoBehaviour
{
	//여기서 처음에 selecter을 쓴다.

	//그 후에 Sequence를 사용해서 state를 계속 실행 시켜준다ㅏ.

	[SerializeField]
	private State _state;

	private bool _isDefend = false;

	private void Start()
	{
		GameManager.Instance.onStateChange += DefendOn;
	}

	public void Update()
	{
		if (_isDefend)
		{
			_isDefend = _state.IsStateOn();
			_state.StateOn();
		}
	}

	private void DefendOn(BattingState state)
	{
		if (state == BattingState.Defending)
		{
			_isDefend = true;
		}
	}
}
