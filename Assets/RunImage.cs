using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RunImage : MonoBehaviour
{
	private Runner _runnerObject;
	public Runner runnerObject => _runnerObject;

	private int _runIndex = 0;
	public int Index => _runIndex;

	private Vector3 _startPos = Vector3.zero;
	private Vector3 _endPos = Vector3.zero;

	bool _isRun = false;
	bool _isHomeRun = false;
	private void Update()
	{
		if(_isRun)
		{
			float originPos = Vector3.Distance(_runnerObject.StartPos, _runnerObject.RunObjectVec);
			float defPos = Vector3.Distance(_runnerObject.StartPos, _runnerObject.transform.position);

			float percentage = defPos / originPos;
			if (percentage >= 0.9)
			{
				if (_runIndex == 3)
					Destroy(this.gameObject);

				_isRun = false;
			}

			Vector3 pos = Vector3.Lerp(_startPos, _endPos, percentage);

			this.gameObject.transform.position = pos;
		}
	}

	public void SetRun(Runner runner,Vector3 startPos, Vector3 endPos, int runIndex)
	{
		_runnerObject = runner;
		_startPos = startPos;
		_endPos = endPos;
		_isRun = true;
		_runIndex = runIndex;
	}

	public void EndRun()
	{
		_isRun = false;
		Destroy(this.gameObject);
	}
}
