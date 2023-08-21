using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;

public class Mitt : MonoBehaviour
{
	[SerializeField]
	private Catcher _catcher;

	private void OnTriggerEnter(Collider other)
	{
		_catcher.Check(other);
	}
}
