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
		if (other.CompareTag("Ball"))
		{
			if (isStrike)
				strike.Strike(other.transform);
			else
				strike.Ball(other.transform);
		}
	}


}