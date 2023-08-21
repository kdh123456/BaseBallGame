using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattingZone : MonoBehaviour
{
	public void OnTriggerEnter(Collider other)
	{
		if(other.gameObject.tag == "Ball")
        {
            GameManager.Instance.ChangeState(BattingState.Batting);
		}
	}
}
