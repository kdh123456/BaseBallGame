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
		GameManager.Instance.onStateChange += TouchOutBase;
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

	public Base ThrowBaseReturn(Base tsBase = null)
	{
		foreach (Base defBase in _bases)
		{
			if (tsBase == defBase)
			{
				continue;
			}

			if(defBase.Running && defBase.HaveDefender)
			{
				return defBase;
			}
		}

		return null;
	}

	public void HomeRun()
	{
		for (int i = 0; i < 4; i++)
		{
			_bases[i].HomeRun();
		}
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

	private void TouchOutBase(BattingState state)
	{
		if (state == BattingState.Batting)
		{
			for (int i = 0; i < 4; i++)
			{
				if(_bases[i].HaveRunner && i > 0)
				{
					_bases[i - 1].OnTouchBase(true);
				}
			}
		}
	}
}
