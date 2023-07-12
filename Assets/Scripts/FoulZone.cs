using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FoulZone : MonoBehaviour
{
	public void OnTriggerEnter(Collider other)
	{
		if(other.CompareTag("Ball") && GameManager.Instance.State == BattingState.Batting)
		{
			if (GameManager.Instance.CurrentStat.strikeCount <= 1)
				GameManager.Instance.AddStrike();
			else
				GameManager.Instance.ChangeState(BattingState.Pitch);
		}
	}
}
