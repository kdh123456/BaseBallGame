using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.UI;

public class BaseControll : MonoSingleton<BaseControll>
{
	[SerializeField]
	private Base[] _bases;

	public Base BaseReturn(int index = 1)
	{
		return _bases[index];
	}

	public bool ThrowBaseHave(Base tsBase = null)
	{
		bool isThrowBaseHave = false;
		foreach(Base curBase in _bases)
		{
			if (tsBase == curBase)
				continue;

			if (!curBase.Running)
				continue;

			if(curBase.BaseRunnerDistance() > 1)
			{
				isThrowBaseHave = true;
				break;
			}
		}

		return isThrowBaseHave;
	}

	public Base ThrowBaseReturn()
	{
		Base curBase = null;
		foreach (Base defBase in _bases)
		{
			if (curBase == null)
			{
				curBase = defBase;
				continue;
			}

			if (!curBase.Running && defBase)
			{
				curBase = defBase;
			}

			if (curBase.Running == false)
				continue;
			if (defBase.Running == false)
				continue;

			if (curBase.BaseRunnerDistance() > defBase.BaseRunnerDistance())
				curBase = defBase;
		}

		return curBase;
	}
}
