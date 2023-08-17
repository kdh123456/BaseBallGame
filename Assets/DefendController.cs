using System.Collections;
using System.Collections.Generic;
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
		if (state == BattingState.Defending)
			_catch = false;
	}

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
					GameObject obj = new GameObject("targetPos");
					obj.transform.position = defenExpectMinuVec;
					defen.TargetChase(obj);
				}
			}
		}
	}

	private Defender FindShotDistanceObject(GameObject obj)
	{
		Defender defender = null;
		foreach (Defender defen in _defenders)
		{
			if (defender == null)
				defender = defen;

			float originObjDis = Vector3.Distance(obj.transform.position, defender.transform.position);
			float defrenceObjDis = Vector3.Distance(obj.transform.position, defen.transform.position);

			if (originObjDis > defrenceObjDis)
				defender = defen;
		}

		return defender;
	}

	private void Update()
	{
		if (_catch)
		{


			//Vector3 vec = _expectedpath;
			//vec.y = 1;

			//if(Vector3.Distance(BallObject.battingVec, vec) > 2)
			//{
			//	chasingObject = FindShotDistanceObject(GameManager.Instance.BallObject);
			//	chasingObject.TargetChase(GameManager.Instance.BallObject);
			//}
		}
	}

	private void WeightOfBaseCover()
	{
		if(BaseControll.Instance.BaseIsEmpty())
		{
			List<Base> bases = BaseControll.Instance.EmptyBases();
			foreach(Defender defen in _defenders)
			{
				if (defen.IsBase)
				{
					//defen.weightOfState[DefenderStateEnum.BaseCover]

				}


			}
		}
	}
}
