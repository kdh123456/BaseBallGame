using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RunnerMap : MonoBehaviour
{
	[SerializeField]
	private Image[] _bases;

	[SerializeField]
	private RunImage _runner;

	private List<RunImage> _runners = new List<RunImage>();

	private void Start()
	{
		RunnerManager.Instance.runStart += RunFirst;
		RunnerManager.Instance.runEnd += RunEnd;
		RunnerManager.Instance.onBaseOn += Run;
	}

	public void Run()
	{
		foreach (var run in _runners)
		{
			int index = run.Index;
			int nextIndex = index+1;
			run.SetRun(run.runnerObject, _bases[index].transform.position, _bases[nextIndex].transform.position, nextIndex);
		}
	}

	private void RunFirst()
	{
		GameObject obj = Instantiate(_runner.gameObject, this.transform);
		RunImage newRunObject = obj.GetComponent<RunImage>();
		newRunObject.GetComponent<Image>().rectTransform.anchoredPosition = _bases[3].rectTransform.anchoredPosition;
		newRunObject.SetRun(RunnerManager.Instance.BattingRunner(),_bases[3].transform.position, _bases[0].transform.position, 0);

		Run();

		_runners.Add(newRunObject);
	}

	private void RunEnd(Runner runner)
	{
		foreach (var run in _runners)
		{
			if(run.runnerObject == runner)
			{
				run.EndRun();
			}
		}
	}

	private void Run(Runner runner)
	{
		foreach (var run in _runners)
		{
			if (run.runnerObject == runner)
			{
				int index = run.Index;
				int nextIndex = index + 1;
				run.SetRun(run.runnerObject, _bases[index].transform.position, _bases[nextIndex].transform.position, nextIndex);
			}
		}
	}
}
