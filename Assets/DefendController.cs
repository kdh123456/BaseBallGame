using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DefendController : MonoBehaviour
{
	[SerializeField]
	private Defender[] _defenders;

	private bool _catch = false;

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
	}

	private void StartFind(BattingState state)
	{
		if (state == BattingState.Batting)
			_catch = true;
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
			chasingObject = FindShotDistanceObject(GameManager.Instance.BallObject);
			chasingObject.TargetChase(GameManager.Instance.BallObject);
		}
	}
}
