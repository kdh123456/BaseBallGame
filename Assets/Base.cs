using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Base : MonoBehaviour
{
    [SerializeField]
    private int index;

    public int Index => index;

	private bool _haveRunner = false;

	private Runner _runner;

	private void OnTriggerEnter(Collider other)
	{
		
	}

	public void RunSuccess(Runner runer)
	{
		_haveRunner = true;
		_runner = runer;
	}

	public void ExitBase()
	{
		_haveRunner = false;
		_runner = null;
	}
}
