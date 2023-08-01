using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseControll : MonoSingleton<BaseControll>
{
	[SerializeField]
	private Base[] _bases;

	public Base BaseReturn(int index = 1)
	{
		return _bases[index];
	}
}
