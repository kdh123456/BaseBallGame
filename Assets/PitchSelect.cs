using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum PitchType
{
	FourSeamFastBall,
	TwoSeamFastBall,
	CurveBall,
	SliderBall,
}

public class PitchSelect : MonoBehaviour
{
	[SerializeField]
	private PitchType _type;

	public void SelectType(PitchType type)
	{
		_type = type;
	}
}
