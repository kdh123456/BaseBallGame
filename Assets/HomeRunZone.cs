using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HomeRunZone : MonoBehaviour
{
	public void OnCollisionEnter(Collision collision)
	{
		if (collision.transform.tag == "Ball"
			&& GameManager.Instance.State == BattingState.Batting)
		{
				BaseControll.Instance.HomeRun();
		}
	}
}
