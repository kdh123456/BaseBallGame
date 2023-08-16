using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class Base : MonoBehaviour
{
	[SerializeField]
	private int index;

	public bool _isHomeBase = false;

	public int Index => index;

	private bool _haveRunner = false;
	public bool HaveRunner => _haveRunner;
	public bool Running => _comeRunner != null;

	private bool _baseCover = false;
	public bool IsBaseCover => _baseCover;

	private Runner _inSideRunner;

	private Runner _comeRunner;

	private Defender _defender;

	private void OnTriggerEnter(Collider other)
	{
		CheckRunnerAndDefender(other);
	}

	private void OnTriggerExit(Collider other)
	{
		CheckRunnerAndDefender(other, true);
	}

	private void CheckRunnerAndDefender(Collider col, bool isExit = false)
	{
		Runner runner = col.GetComponent<Runner>();
		Defender defender = col.GetComponent<Defender>();

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

	private void EnterRunnerAndDefender(bool isDefender, Runner run, Defender defen)
	{
		if (isDefender)
		{
			_defender = defen;
			_defender?.BaseIn(this);
		}
		else
		{
			if (_defender != null)
			{
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

	private void ExitRunnerAndDefender(bool isDefender, Runner run, Defender defen)
	{
		if (isDefender)
		{
			_defender?.BaseOut();
			_defender = null;
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
		if(_isHomeBase)
		{
			GameManager.Instance.AddScore();
			runer?.gameObject?.SetActive(false);
		}
		_haveRunner = true;
		_inSideRunner = runer;
	}

	private void RunFailed(Runner runer)
	{
		runer.gameObject.SetActive(false);
		_inSideRunner = null;
		GameManager.Instance.AddOut();
		_defender.BaseOut();
	}

	public void ExitBase()
	{
		_haveRunner = false;
		_inSideRunner = null;
	}

	public float BaseRunnerDistance()
	{
		return Vector3.Distance(this.transform.position, _comeRunner.transform.position);
	}                                                                            
}
