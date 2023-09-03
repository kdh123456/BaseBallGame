using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.UI;

public class BaseControll : MonoSingleton<BaseControll>
{
	[SerializeField]
	private Base[] _bases;

	private Base _homeRunBase;

	public Base HomeRunBase => _homeRunBase;

	private Runner _homeRunRunner;

	public Runner HomrRunRunner => _homeRunRunner;

	public event Action _homeRun;

	public void Start()
	{
		for (int i = 0; i < 3; i++)
		{
			if (_bases[i]._isHomeBase)
			{
				_homeRunBase = _bases[i];
				break;
			}
		}

		GameManager.Instance.onStateChange += HomeRunEnd;
		GameManager.Instance.onHomeRun += HomeRun;
	}

	public Base BaseReturn(int index = 1)
	{
		return _bases[index];
	}

	public bool BaseIsEmpty()
	{
		foreach (Base bases in _bases)
		{

			if (bases.IsBaseCover || bases.HaveDefender)
				continue;

			return true;
		}

		return false;
	}

	public List<Base> EmptyBases()
	{
		List<Base> baseList = new List<Base>();
		foreach (Base bases in _bases)
		{

			if (bases.IsBaseCover || bases.HaveDefender)
				continue;

			baseList.Add(bases);
		}

		return baseList;
	}

	public bool BaseComeRunnerHave()
	{
		foreach (Base bases in _bases)
		{
			if(bases._isHomeBase)
				continue;

			if (bases.Running || bases.HaveRunner)
				return true;
		}

		return false;
	}

	public bool ThrowBaseHave(Base tsBase = null)
	{
		foreach (Base curBase in _bases)
		{
			if (tsBase == curBase)
			{
				continue;
			}

			if (curBase.Running && curBase.HaveDefender)
			{
				return true;
			}
		}

		return false;
	}

	public Base ThrowBaseReturn(Vector3 vec, Base tsBase = null)
	{
		float distance = 999f;
		Base bases = null;
		foreach (Base defBase in _bases)
		{
			if (tsBase == defBase)
			{
				continue;
			}

			float dis = Vector3.Distance(defBase.transform.position, vec);

			if (distance > Vector3.Distance(defBase.transform.position, vec) && defBase.Running && defBase.HaveDefender)
			{
				bases = defBase;
				distance = dis;
			}
		}

		return bases;
	}

	public void HomeRun()
	{
		for (int i = 0; i < 4; i++)
		{
			_bases[i].HomeRun();
		}
		_homeRunRunner = RunnerManager.Instance.BattingRunner();
	}

	private void HomeRunEnd(BattingState state)
	{
		if (state == BattingState.Idle)
		{
			for (int i = 0; i < 4; i++)
			{
				_bases[i].HomeRunEnd();
			}
		}
	}

	public void TouchOutBase()
	{
		_bases[0].OnTouchBase(true);
		for (int i = 1; i < 4; i++)
		{
			if (_bases[i-1].HaveRunner)
			{
				_bases[i].OnTouchBase(true);
			}
		}
	}
}
