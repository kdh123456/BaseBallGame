using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StrikeZone : MonoBehaviour
{
	[SerializeField]
	private AddBallImage image;

	public bool countEnd = false;

	[NonSerialized]
	public bool isSktrike = false;

	public void Start()
	{
		GameManager.Instance.onStateChange += (state) =>
		{
			if (state == BattingState.Pitch)
			{
				countEnd = false;
			}
		};
	}

	public void Strike(Transform trans)
	{
		if (countEnd)
			return;

		isSktrike = true;

		countEnd = true;

		Debug.Log("Strike");
	}
	public void Ball(Transform trans)
	{
		if (countEnd)
			return;

		isSktrike = false;
		Debug.Log("Ball");
	}
}
