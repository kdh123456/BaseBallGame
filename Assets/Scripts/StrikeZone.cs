using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StrikeZone : MonoBehaviour
{
	[SerializeField]
	private AddBallImage image;

	public bool countEnd;

	public void Strike(Transform trans)
	{
		image.AddBall(trans);
		GameManager.Instance.AddStrike();
		countEnd = true;
	}
	public void Ball(Transform trans)
	{
		if(countEnd)
		{
			countEnd = false;
			return;
		}
		image.AddBall(trans);
		GameManager.Instance.AddBall();
	}
}
