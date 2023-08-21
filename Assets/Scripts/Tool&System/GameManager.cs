using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum BattingState
{
	Bat,
	Batting,
	Defending,
	Pitching,
	Pitch,
	Idle,
}


public enum Mode
{
	PitchMode,
	BatMode
}

public enum Count
{
	Strike,
	Ball
}

[Serializable]
public class TeamStat
{
	//public string teamName;
	public int teamScore = 0;
	public int playerIndex = 1;
	public int outCount = 0;
	public int strikeCount = 0;
	public int ballCount = 0;

	public TeamEnum teamName;
}

public class GameManager : MonoSingleton<GameManager>
{
	[SerializeField]
	private BattingState _state;

	public BattingState State => _state;

	public Mode gameMode = Mode.PitchMode;

	public TeamStat CurrentStat => currentTeam;

	private TeamStat currentTeam;

	[SerializeField]
	private TeamStat firstTeam = new TeamStat();

	[SerializeField]
	private TeamStat secoundTeam = new TeamStat();

	public event Action<CountEnum> onChangeCount;
	public event Action<Mode> onChangeGameMode;
	public event Action<BattingState> onStateChange;
	public event Action<TeamEnum, int> onAddScore;

	private bool isChange = false;

	[SerializeField]
	private GameObject _runnerObject;

	public GameObject RunnerObject => _runnerObject;

	private GameObject _ballObject;

	public GameObject BallObject => _ballObject;

	private void Start()
	{
		StartTeam();
		ChangeMode(Mode.PitchMode);

		onChangeCount += ChangeTeam;
		onStateChange += IdleAction;
	}

	private void Update()
	{
		if(Input.GetKeyDown(KeyCode.C))
		{
			if(!isChange)
				ChangeMode(Mode.BatMode);
			else
				ChangeMode(Mode.PitchMode);

			isChange = !isChange;
		}
	}

	public void StartTeam()
	{
		firstTeam.strikeCount = 0;
		firstTeam.ballCount = 0;
		firstTeam.outCount = 0;
		firstTeam.playerIndex = 0;

		currentTeam = firstTeam;
	}

	public void ChangeMode(Mode mode)
	{
		gameMode = mode;
		onChangeGameMode?.Invoke(gameMode);
	}

	public void ChangeState(BattingState state)
	{
		_state = state;
		onStateChange?.Invoke(state);
	}

	public void ChangeTeam(CountEnum count)
	{
		if (count == CountEnum.Out && currentTeam.outCount == 3)
		{
			onChangeCount?.Invoke(CountEnum.OutReset);
			currentTeam.outCount = 0;
			currentTeam.strikeCount = 0;
			currentTeam.ballCount = 0;
			currentTeam = currentTeam == firstTeam ? secoundTeam : firstTeam;
			ChangeMode(gameMode == Mode.PitchMode ? Mode.BatMode : Mode.PitchMode);
		}
	}

	public void ChangeTeamBatter()
	{
		currentTeam.strikeCount = 0;
		currentTeam.ballCount = 0;
		Debug.Log("Change");
		onChangeCount?.Invoke(CountEnum.Reset);
		currentTeam.playerIndex++;
	}

	public void AddStrike()
	{
		_state = BattingState.Pitch;
		if (currentTeam.strikeCount == 2)
		{
			currentTeam.outCount++;
			onChangeCount?.Invoke(CountEnum.Out);
			ChangeTeamBatter();
			return;
		}
		currentTeam.strikeCount++;
		//ChangeState(BattingState.Idle);
		onChangeCount?.Invoke(CountEnum.Strike);
	}

	public void AddBall()
	{
		if (currentTeam.ballCount == 3)
		{
			ChangeTeamBatter();
			return;
		}
		currentTeam.ballCount++;
		//ChangeState(BattingState.Idle);
		onChangeCount?.Invoke(CountEnum.Ball);
	}

	public void AddOut()
	{
		currentTeam.outCount++;
		onChangeCount?.Invoke(CountEnum.Out);
		//ChangeState(BattingState.Idle);
	}

	public void AddScore ()
	{
		currentTeam.teamScore++;
		onAddScore?.Invoke(currentTeam.teamName, 1);
		//ChangeState(BattingState.Idle);
	}

	public void SetBall(GameObject obj)
	{
		_ballObject = obj;
	}

	public void IdleAction(BattingState state)
	{
		if(state == BattingState.Idle)
		{
			StartCoroutine(WaitForMode(1f, gameMode));
		}
	}

	private IEnumerator WaitForMode(float timer, Mode mode)
	{
		yield return new WaitForSeconds(timer);
		if (mode == Mode.PitchMode)
			PitchMode();
		else
			BatMode();
	}

	public void BatMode()
	{
		CameraController.Instance.ChangeCameraPos(Mode.BatMode);
		BattingManager.Instance.BattingReset();
	}

	public void PitchMode()
	{
		CameraController.Instance.ChangeCameraPos(Mode.PitchMode);
	}

	public void ResetGame(BattingState state)
	{
		if (state == BattingState.Idle)
		{
			_ballObject = null;
		}
	}
}
