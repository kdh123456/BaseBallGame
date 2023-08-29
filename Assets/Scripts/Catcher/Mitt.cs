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
		if (GameManager.Instance.State == BattingState.Pitching || GameManager.Instance.State == BattingState.Bat)
		{
			_catcher.Check(other);
			GameManager.Instance.WaitReset();
			Destroy(other);
		}
	}
}
