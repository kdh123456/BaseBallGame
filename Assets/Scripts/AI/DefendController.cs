using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.AI;

public enum DefendZoneEnum
{
	InsideField,
	OutCenterField,
	OutLeftField,
	OutRighttField
}

public class DefendController : MonoSingleton<DefendController>
{

	public Defend[] _defends;
	private Ball BallObject => GameManager.Instance.BallObject.GetComponent<Ball>();
	private Defend _comeBallDefender;
	private BallChaseState _comeBallDefenderChaseState;
	private float coverBallDefenderDistance = 999f;

	private bool _defendOn = false;

	[SerializeField]
	private GameObject[] objs;

	public Action onBating;

	private Ball _ball; 
	private void Start()
	{
		GameManager.Instance.onStateChange += BallChaseOn;
		GameManager.Instance.onStateChange += BallChaseOff;
	}

	private void Update()
	{
		if (_defendOn)
		{	
			if(!GameManager.Instance.IsHomeRun)
			{
				BallCover();
				BaseCover();
				if (RunnerManager.Instance.AllRunnerRuningEnd())
					GameManager.Instance.WaitReset();
			}
		}
	}
	public void BallCover()
	{
		float minDistance = 999f;
		Defend coverDefend = null;

		if (_ball == null)
			return;

		if (_ball.IsMuzzle || _ball.IsThrowing)
		{
				if (_comeBallDefender != null)
				_comeBallDefenderChaseState.ballCoverOn = false;
			return;
		}

		foreach (Defend defender in _defends)
		{
			if (defender.GetComponent<BaseCoverState>().CoverBase != null)
				continue;

			float distance = Vector3.Distance(GameManager.Instance.BallObject.transform.position, defender.transform.position);
			if (minDistance > distance)
			{
				minDistance = distance;
				coverDefend = defender;
			}
		}

		if (_comeBallDefender != null)
			_comeBallDefenderChaseState.ballCoverOn = false;

		coverBallDefenderDistance = minDistance;
		_comeBallDefender = coverDefend;
		_comeBallDefenderChaseState = _comeBallDefender.GetComponent<BallChaseState>();
		coverDefend.GetComponent<BallChaseState>().ballCoverOn = true;
	}

	public void BaseCover()
	{
		float minDistance = 999f;
		Defend coverDefend = null;
		List<Base> _base = BaseControll.Instance.EmptyBases();

		for(int i = 0; i< _base.Count; i++)
		{
			foreach (Defend defender in _defends)
			{
				if (defender.GetComponent<BallChaseState>().ballCoverOn)
					continue;

				float distance = Vector3.Distance(_base[i].transform.position, defender.transform.position);
				if (minDistance > distance)
				{
					minDistance = distance;
					coverDefend = defender;
				}
			}
			minDistance = 999f;

			coverDefend.GetComponent<BaseCoverState>().baseCoverOn = true;
			coverDefend.GetComponent<BaseCoverState>()._coverBase = _base[i];
		}
	}

	public void PathFoulCheck(Ball ball)
	{
		Vector3 targetDir = ball.FuturePath - this.transform.position; // 타겟 방향으로 향하는 벡터를 구하기
		Vector3 dis1 = objs[3].transform.position - objs[2].transform.position;
		Vector3 dis2 = objs[5].transform.position - objs[4].transform.position;
		Vector3 crossVec1 = Vector3.Cross(targetDir, dis1); // 포워드와 외적
		Vector3 crossVec2 = Vector3.Cross(targetDir, dis2); // 포워드와 외적

		Debug.DrawLine(objs[0].transform.position, dis1.normalized * 100);
		Debug.DrawLine(objs[2].transform.position, dis2.normalized * 100);

		float dot1 = Vector3.Dot(crossVec1, Vector3.up);
		float dot2 = Vector3.Dot(crossVec2, Vector3.up);

		Vector3 dis0 = objs[1].transform.position - objs[0].transform.position;
		Vector3 targetDir0 = ball.FuturePath - objs[0].transform.position;
		Vector3 crossVec0 = Vector3.Cross(targetDir0, dis0);

		float dot0 = Vector3.Dot(crossVec0, Vector3.up);

		if (dot1 > 0 || dot2 < 0 || dot0 < 0)
		{
			if (GameManager.Instance.CurrentStat.strikeCount < 2)
			{
				GameManager.Instance.AddStrike();
				GameManager.Instance.WaitReset();
			}
			else
			{
				GameManager.Instance.WaitReset();
			}
		}
		else if(dot0 > 0 && dot1 < 0 && dot2 > 0)
		{
			BaseControll.Instance.TouchOutBase();
			GameManager.Instance.ChangeState(BattingState.Batting);
		}
	}

	public void FuturePathCover()
	{
		float minDistance = 999f;
		Defend coverDefend = null;
		Ball ball = GameManager.Instance.BallObject.GetComponent<Ball>();

		PathFoulCheck(ball);

		foreach (Defend defender in _defends)
		{
			float distance = Vector3.Distance(ball.FuturePath, defender.transform.position);
			if (minDistance > distance)
			{
				minDistance = distance;
				coverDefend = defender;
			}
		}
		coverDefend.GetComponent<FuturePathState>().FutureSet(ball.FuturePath);
	}

	private void BallChaseOn(BattingState state)
	{
		if (state == BattingState.Batting)
		{
			_defendOn = true;
			_ball = GameManager.Instance.BallObject.GetComponent<Ball>();
			_ball = GameManager.Instance.BallObject.GetComponent<Ball>();
		}
	}

	private void BallChaseOff(BattingState state)
	{
		if(state == BattingState.Idle)
		{
			_defendOn = false;
		}
	}
}
 