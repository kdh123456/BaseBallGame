using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BatterAI : MonoBehaviour
{
	[SerializeField]
	private State _batState;

	bool _isBatting = false;
	private void Update()
	{
		_isBatting = _batState.IsStateOn();
		if(_isBatting && GameManager.Instance.gameMode == Mode.PitchMode)
		{
			Debug.Log("sdaf");
			_batState.StateOn();
		}
	}
}
