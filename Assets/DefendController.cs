using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public enum DefendZoneEnum
{
	InsideField,
	OutCenterField,
	OutLeftField,
	OutRighttField
}

public class DefendController : MonoBehaviour
{
	[SerializeField]
	private Defender[] _defenders;
	[SerializeField]
	private Collider[] _cols;

	private bool _catch = false;

	private Ball BallObject => GameManager.Instance.BallObject.GetComponent<Ball>();

	private Vector3 _expectedpath = Vector3.zero;

	private Defender _chasingObject;
	private Defender chasingObject
	{
		get { return _chasingObject; }
		set
		{
			if (_chasingObject != value)
			{
				_chasingObject?.TargetChaseStop();
				_chasingObject = value;
			}
		}

	}

	private void Start()
	{
		GameManager.Instance.onStateChange += StartFind;
		GameManager.Instance.onStateChange += EndFind;
		GameManager.Instance.onStateChange += BattingAndFindShotDistancObjects;
	}

	private void StartFind(BattingState state)
	{
		if (state == BattingState.Batting)
			_catch = true;
	}
	private void EndFind(BattingState state)
	{
		if (state == BattingState.Idle)
			_catch = false;
	}

	#region 개발중

	//이게 미래 예측
	private void BattingAndFindShotDistancObjects(BattingState state)
	{
		if (state == BattingState.Batting)
		{
			Ball ball = BallObject;
			_expectedpath = ball.battingVec;

			foreach (Defender defen in _defenders)
			{
				Vector3 defenVec = defen.transform.position;
				Vector3 defenExpectMinuVec = new Vector3(_expectedpath.x * Mathf.Abs(defenVec.z), 1, defenVec.z);

				if (Vector3.Distance(defenVec, defenExpectMinuVec) < 10)
				{
					bool isStateHave = defen.weightOfState.ContainsKey(DefenderStateEnum.FuturePathChase);
					if(isStateHave)
					{
						defen.weightOfState[DefenderStateEnum.FuturePathChase] = (float)Vector3.Distance(defenVec, defenExpectMinuVec);
						defen.FuturePathSetting(defenExpectMinuVec);
					}
					else
					{

					}
				}
			}
		}
	}

	//공 따라가기
	private void BallCover()
	{
		Defender defender = FindShotDistanceObject(BallObject.gameObject);

		if (defender.HaveBall)
			defender.TargetChase(BallObject.gameObject);

		return;
	}

	//이게 베이스 커버
	private void WeightOfBaseCover()
	{
		if (BaseControll.Instance.BaseIsEmpty())
		{
			List<Base> bases = BaseControll.Instance.EmptyBases();

			for (int i = 0; i < bases.Count; i++)
			{
				Defender defens = null;
				float minDistance = 999f;
				foreach (Defender defen in _defenders)
				{
					if (defen.IsBase)
						continue;

					if (defens == null)
						defens = defen;

					float Distance = Vector3.Distance(defens.transform.position, bases[i].transform.position);

					if (minDistance > Distance)
					{
						minDistance = Distance;
						defens = defen;
					}
				}

				defens.weightOfState[DefenderStateEnum.BaseCover] = (float)minDistance;
				defens.BaseCover(bases[i].gameObject);
			}

		}
	}

	#endregion

	private void Update()
	{
		if (_catch)
		{
			BallCover();
			WeightOfBaseCover();
		}
	}

	private Defender FindShotDistanceObject(GameObject obj)
	{
		Defender defender = null;
		foreach (Defender defen in _defenders)
		{
			if (defender == null)
				defender = defen;

			if (defender.HaveBall)
				return defen;

			float originObjDis = Vector3.Distance(obj.transform.position, defender.transform.position);
			float defrenceObjDis = Vector3.Distance(obj.transform.position, defen.transform.position);

			defen.weightOfState[DefenderStateEnum.BallChase] = defrenceObjDis;

			if (originObjDis > defrenceObjDis)
				defender = defen;
		}

		return defender;
	}
}
