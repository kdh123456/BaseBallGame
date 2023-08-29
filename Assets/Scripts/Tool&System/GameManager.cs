using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum BattingState
{
	Bat,
	Batting,
	Pitching,
	Pitch,
	Idle,
}


public enum Mode
{
	PitchMode = 0,
	BatMode,
	EndMode,
	StartMode,
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
	public event Action<TeamEnum, int> onChangeTeam;
	public event Action onHomeRun;

	private bool isChange = false;
	private bool _isHomeRun = false;
	public bool IsHomeRun => _isHomeRun;

	[SerializeField]
	private GameObject _runnerObject;

	public GameObject RunnerObject => _runnerObject;

	private GameObject _ballObject;

	public GameObject BallObject => _ballObject;

	private int episode = 8;
	private int Episode => episode;

	private void Start()
	{
		StartTeam();
		ChangeMode(Mode.StartMode);

		onChangeCount += ChangeTeam;
		onStateChange += IdleAction;
		onStateChange += DelBall;
	}

	private void Update()
	{
		if (Input.GetKeyDown(KeyCode.C))
		{
			if (!isChange)
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
			currentTeam.outCount = 0;
			currentTeam.strikeCount = 0;
			currentTeam.ballCount = 0;
			currentTeam = currentTeam == firstTeam ? secoundTeam : firstTeam;
			onChangeCount?.Invoke(CountEnum.OutReset);
			if(episode == 9)
			{
				ChangeMode(Mode.EndMode);
				return;
			}
			ChangeMode(gameMode == Mode.PitchMode ? Mode.BatMode : Mode.PitchMode);
			episode++;
		}
	}

	public void ChangeTeamBatter()
	{
		currentTeam.strikeCount = 0;
		currentTeam.ballCount = 0;
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
		onChangeCount?.Invoke(CountEnum.Ball);
	}

	public void AddOut()
	{
		currentTeam.outCount++;
		onChangeCount?.Invoke(CountEnum.Out);
	}

	public void AddScore()
	{
		currentTeam.teamScore++;
		onAddScore?.Invoke(currentTeam.teamName, 1);
	}

	public void SetBall(GameObject obj)
	{
		_ballObject = obj;
	}

	public void DelBall(BattingState state)
	{
		if (state == BattingState.Idle)
			Destroy(_ballObject);
	}

	public void IdleAction(BattingState state)
	{
		if (state == BattingState.Idle)
		{
			if (gameMode == Mode.PitchMode)
				PitchMode();
			else
				BatMode();

			_isHomeRun = false;
			CameraController.Instance.CameraReset();
		}
	}

	public void ChangeTeam(TeamEnum teamEnum, bool isTeam = false)
	{
		if(!isTeam)
		{
			firstTeam.teamName = teamEnum;
			onChangeTeam?.Invoke(firstTeam.teamName, 1);
		}
		else
		{
			secoundTeam.teamName = teamEnum;
			onChangeTeam?.Invoke(secoundTeam.teamName, 0);
		}
	}

	public void BatMode()
	{
		CameraController.Instance.ChangeCameraPos(Mode.BatMode);
	}

	public void PitchMode()
	{
		CameraController.Instance.ChangeCameraPos(Mode.PitchMode);
	}

	public void WaitReset()
	{
		Invoke("ResetMode", 3f);
	}

	private void ResetMode()
	{
		ChangeState(BattingState.Idle);
	}

	public void ResetGame(BattingState state)
	{
		if (state == BattingState.Idle)
		{
			_ballObject = null;
		}
	}

	public void HomeRun()
	{
		_isHomeRun = true;
		onHomeRun?.Invoke();
	}
}
