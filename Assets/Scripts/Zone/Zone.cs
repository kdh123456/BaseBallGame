using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Zone : MonoBehaviour
{
	public bool isStrike = true;

	private StrikeZone strike;

	private void Awake()
	{
		strike = gameObject.GetComponentInParent<StrikeZone>();
	}

	public void OnTriggerEnter(Collider other)
	{
		if (other.CompareTag("Ball") && GameManager.Instance.State == BattingState.Pitching)
		{
			if (!other.GetComponent<Ball>().IsShoot)
				return;

			Debug.Log(other.GetComponent<Ball>().IsShoot);
			Debug.Break();


			Debug.Log("BallEnter");
			if (isStrike)
				strike.Strike(other.transform);
			else
				strike.Ball(other.transform);
		}
	}


}
