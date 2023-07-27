using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum BattingState
{
	Batting,
	Pitching,
	Pitch,
	Idle,
}


public enum Mode
{
	PitchMode,
	BatMode
}

[Serializable]
public class TeamStat
{
	public string teamName;
	public int teamScore = 0;
	public int playerIndex = 1;
	public int outCount = 0;
	public int strikeCount = 0;
	public int ballCount = 0;
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

	public GameObject ballObject;

	private bool isChange = false;

	private void Start()
	{
		StartTeam();
		ChangeMode(Mode.PitchMode);

		onChangeCount += ChangeTeam;
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
}
