using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class Defend : MonoBehaviour
{
	//���⼭ ó���� selecter�� ����.

	//�� �Ŀ� Sequence�� ����ؼ� state�� ��� ���� �����ش٤�.

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
