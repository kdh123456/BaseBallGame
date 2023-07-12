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

	public TeamStat CurrentStat => currentTeam;

	private TeamStat currentTeam;

	[SerializeField]
	private TeamStat firstTeam = new TeamStat();

	[SerializeField]
	private TeamStat secoundTeam = new TeamStat();

	public event Action<CountEnum> onChangeCount;
	public GameObject ballObject;

	private void Start()
	{
		StartTeam();
	}

	public void StartTeam()
	{
		firstTeam.strikeCount = 0;
		firstTeam.ballCount = 0;
		firstTeam.outCount = 0;
		firstTeam.playerIndex = 0;

		currentTeam = firstTeam;
	}
	public void EndTeam()
	{

	}

	public void ChangeState(BattingState state)
	{
		_state = state;
	}

	public void ChangeTeam()
	{
		currentTeam.outCount = 0;
		currentTeam.strikeCount = 0;
		currentTeam.ballCount = 0;
		currentTeam = currentTeam == firstTeam ? secoundTeam : firstTeam;
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
		if(currentTeam.strikeCount == 2)
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
