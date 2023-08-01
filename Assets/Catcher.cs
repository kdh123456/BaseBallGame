using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Catcher : MonoBehaviour
{
	[SerializeField]
	private StrikeZone zone;

	public void Check(Collider other)
	{
		if (other.CompareTag("Ball") && GameManager.Instance.State != BattingState.Batting)
		{
			if (zone.isSktrike)
				Strike();
			else if (!zone.isSktrike && GameManager.Instance.State == BattingState.Bat)
				Strike();
			else if (!zone.isSktrike)
				Ball();
		}
	}

	private void Strike()
	{
		GameManager.Instance.AddStrike();
	}

	private void Ball()
	{
		GameManager.Instance.AddBall();
	}
}
