using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RunnerManager : MonoSingleton<RunnerManager>
{
	private List<Runner> runners = new List<Runner>();

	public event Action runStart;
	public event Action<Runner> runEnd;

	public Action<Runner> onBaseOn;
	private void Start()
	{
		
	}

	public void AddRunner(Runner runner)
	{
		runners.Add(runner);
		runStart?.Invoke();
	}
	public void RemoveRunner(Runner runner)
	{
		runners.Remove(runner);
		runEnd?.Invoke(runner);
	}

	public bool AllRunnerRuningEnd()
	{
		foreach(var runner in runners) 
		{
			if (runner.IsRun)
				return false;
		}

		return true;
	}

	public void OutRunner(Runner runner = null)
	{
		if (runner == null)
		{
			if (runners.Count >= 1)
			{
				runners[runners.Count - 1].Out();
			}
		}
		else
			runner.Out();
	}

	public Runner ReturnRunner(int index)
	{
		if (runners.Count <= index)
			return null;
		else
			return runners[index];
	}

	public Runner BattingRunner()
	{
		if (runners.Count >= 1)
			return runners[runners.Count - 1];
		else
			return null;
	}
}
