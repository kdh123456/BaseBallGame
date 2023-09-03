using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Runtime.CompilerServices;
using UnityEngine;

public class Base : MonoBehaviour
{
	[SerializeField]
	private int index;
	public int Index => index;

	public GameObject enemybasePos;
	public GameObject playerbasePos;

	public bool HaveRunner => _inSideRunner != null;
	public Runner runner => _inSideRunner;
	public Runner _inSideRunner;

	public Runner _comeRunner = null;
	public bool Running => _comeRunner != null;

	private Defend _defender;
	public bool HaveDefender => _defender != null;

	public bool haveDefend;


	private bool _baseCover = false;
	public bool IsBaseCover => _baseCover;
	private Defend _comeDefender;

	private bool _isHomeRun = false;
	public bool _isHomeBase = false;

	public bool _isOnTouchAndOutBase = false;
	public bool _touch = false;
	public bool OnAOutBase => _isOnTouchAndOutBase;

	private void Start()
	{
		GameManager.Instance.onStateChange += ResetBase;
	}

	private void Update()
	{
		if (_comeDefender != null)
			Debug.Log(this.gameObject.name + "      " + _comeDefender.name);

		haveDefend = HaveDefender;
	}
	private void OnTriggerEnter(Collider other)
	{
		CheckRunnerAndDefender(other);
	}

	private void OnTriggerStay(Collider other)
	{
		if (_defender != null)
			if (_defender.HaveBall)
				OnTouch();
	}

	private void OnTriggerExit(Collider other)
	{
		CheckRunnerAndDefender(other, true);
	}

	private void CheckRunnerAndDefender(Collider col, bool isExit = false)
	{
		if (col.tag != "BaseBallPlayer")
			return;

		Runner runner = col.GetComponent<Runner>();
		Defend defender = col.GetComponent<Defend>();

		bool isdefender = runner == null ? true : false;

		if (!isExit)
		{
			EnterRunnerAndDefender(isdefender, runner, defender);
		}
		else
		{
			ExitRunnerAndDefender(isdefender, runner, defender);
		}
	}

	private void EnterRunnerAndDefender(bool isDefender, Runner run, Defend defen)
	{
		if (isDefender)
		{
			_defender = defen;
			_baseCover = false;
			_comeDefender = null;

			BaseCoverState state = _defender.GetComponent<BaseCoverState>();
			state.BaseIn();
		}
		else
		{
			if (_defender != null)
			{
				if(_touch && _isOnTouchAndOutBase)
				{
					RunFailed(run);
					return;
				}

				if (_defender.HaveBall == null)
					RunSuccess(run);
				else
					RunFailed(run);
			}
			else
			{
				RunSuccess(run);
			}
		}
	}

	private void ExitRunnerAndDefender(bool isDefender, Runner run, Defend defen)
	{
		if (isDefender)
		{
			if (_defender != null)
			{
				BaseCoverState state = _defender.GetComponent<BaseCoverState>();
				state?.BaseOut();
				_defender = null;
			}
		}
		else
		{
			_inSideRunner = null;
		}
	}

	public void BaseCovering() =>
		_baseCover = true;

	public void BaseRunRunner(Runner runner)
		=> _comeRunner = runner;

	public void RunSuccess(Runner runer)
	{
		if (_isHomeRun)
		{
			if (!_isHomeBase)
				runer?.HomeRun();

			if (_isHomeBase)
			{
				if (runer == BaseControll.Instance.HomrRunRunner)
					GameManager.Instance.WaitReset();
			}
		}
		if (_isHomeBase)
		{
			GameManager.Instance.AddScore();
			runer.DestroyRunner();
		}
		_inSideRunner = runer;

	}

	private void RunFailed(Runner runer)
	{
		runer.Out();
		_inSideRunner = null;
		_comeRunner = null;
		GameManager.Instance.AddOut();
	}

	public void ComeRunFailed() => _comeRunner = null;

	public void ExitBase()
	{
		_inSideRunner = null;
	}
	public float BaseRunnerDistance()
	{
		return Vector3.Distance(this.transform.position, _comeRunner.transform.position);
	}

	public void HomeRun()
		=> _isHomeRun = true;
	public void HomeRunEnd()
	=> _isHomeRun = false;

	public void OnTouchBase(bool isOnTouch)
	{
		_isOnTouchAndOutBase = isOnTouch;
	}

	public void OnTouch()
	{
		if(_isOnTouchAndOutBase)
		{
			_touch = true;
		}
	}


	private void ResetBase(BattingState state)
	{
		if (state == BattingState.Idle)
		{
			//_inSideRunner = null;
			//_comeRunner = null;
			//_defender = null;
			_baseCover = false;
			_comeDefender = null;
			_isHomeRun = false;
			_isOnTouchAndOutBase = false;
			_touch = false;
		}
	}
}
